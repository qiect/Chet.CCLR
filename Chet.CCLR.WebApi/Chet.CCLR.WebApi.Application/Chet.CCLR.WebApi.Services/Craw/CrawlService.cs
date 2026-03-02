using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Data;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;
using Microsoft.EntityFrameworkCore;
using Chet.CCLR.WebApi.DTOs.Craw;

namespace Chet.CCLR.WebApi.Services.Craw;

/// <summary>
/// 爬虫服务实现
/// </summary>
public class CrawlService : ICrawlService
{
    private readonly AppDbContext _context;
    private readonly IClassicBookRepository _bookRepository;
    private readonly IClassicChapterRepository _chapterRepository;
    private readonly IClassicSentenceRepository _sentenceRepository;

    public CrawlService(
        AppDbContext context,
        IClassicBookRepository bookRepository,
        IClassicChapterRepository chapterRepository,
        IClassicSentenceRepository sentenceRepository)
    {
        _context = context;
        _bookRepository = bookRepository;
        _chapterRepository = chapterRepository;
        _sentenceRepository = sentenceRepository;
    }

    public async Task<IEnumerable<CrawlSourceConfig>> GetAllSourcesAsync(CancellationToken cancellationToken = default)
    {
        var sources = new List<CrawlSourceConfig>
        {
            new CrawlSourceConfig
            {
                Id = "hancheng",
                Name = "汉程国学",
                Type = "html",
                BaseUrl = "https://guoxue.httpcn.com",
                CatalogUrl = "https://guoxue.httpcn.com/book/daodejing/",
                DetailUrlPattern = "https://guoxue.httpcn.com/html/book/TBXVKOPW/CQXVTBAZAZKO.shtml",
                DelayMilliseconds = 1000
            }
        };

        return sources;
    }

    public async Task<CrawlSourceConfig?> GetSourceByIdAsync(string sourceId, CancellationToken cancellationToken = default)
    {
        var sources = await GetAllSourcesAsync(cancellationToken);
        return sources.FirstOrDefault(s => s.Id == sourceId);
    }

    public async Task<CrawlResult> CrawlAsync(CrawlTaskRequest request, CancellationToken cancellationToken = default)
    {
        var result = new CrawlResult();

        try
        {
            var sourceConfig = await GetSourceByIdAsync(request.SourceId, cancellationToken);
            if (sourceConfig == null)
            {
                result.Success = false;
                result.Message = "数据源不存在";
                return result;
            }

            var crawler = CreateCrawler(sourceConfig.Type, sourceConfig.Id);
            if (crawler == null)
            {
                result.Success = false;
                result.Message = "不支持的数据源类型";
                return result;
            }

            result = await crawler.CrawlCatalogAsync(sourceConfig.CatalogUrl, cancellationToken);

            if (result.Success && result.Chapters.Any())
            {
                for (int i = 0; i < result.Chapters.Count; i++)
                {
                    var chapter = result.Chapters[i];
                    var chapterResult = await crawler.CrawlChapterAsync(sourceConfig.CatalogUrl, chapter.DetailUrl ?? string.Empty, cancellationToken);

                    if (chapterResult.Success)
                    {
                        result.Sentences.AddRange(chapterResult.Sentences);
                    }

                    await Task.Delay(sourceConfig.DelayMilliseconds, cancellationToken);
                }
            }

            result.Message = $"爬取完成：{result.Chapters.Count}章，{result.Sentences.Count}句";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"爬取失败：{ex.Message}";
        }

        return result;
    }

    public async Task<CrawlResult> ImportToDatabaseAsync(CrawlResult result, bool overwrite, string audioDirectory, CancellationToken cancellationToken = default)
    {
        if (!result.Success)
        {
            return result;
        }

        try
        {
            if (result.Books.Count == 0)
            {
                result.Success = false;
                result.Message = "没有书籍数据";
                return result;
            }

            var book = result.Books[0];
            var existingBook = await _bookRepository.GetByTitleAsync(book.Title, cancellationToken);

            if (existingBook != null && !overwrite)
            {
                result.Success = false;
                result.Message = "书籍已存在，如需覆盖请设置 overwrite=true";
                return result;
            }

            if (existingBook == null)
            {
                existingBook = new ClassicBook
                {
                    Id = Guid.CreateVersion7(),
                    Title = book.Title,
                    Author = book.Author,
                    Dynasty = book.Dynasty,
                    Category = book.Category,
                    Description = book.Description,
                    CoverImage = book.CoverImage,
                    TotalChapters = result.Chapters.Count,
                    TotalSentences = result.Sentences.Count,
                    Level = 3,
                    IsPublished = true,
                    OrderIndex = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _bookRepository.AddAsync(existingBook, cancellationToken);
            }
            else if (overwrite)
            {
                existingBook.TotalChapters = result.Chapters.Count;
                existingBook.TotalSentences = result.Sentences.Count;
                existingBook.UpdatedAt = DateTime.Now;
                await _bookRepository.UpdateAsync(existingBook, cancellationToken);

                await DeleteExistingDataAsync(existingBook.Id, cancellationToken);
            }

            foreach (var chapterDto in result.Chapters)
            {
                var existingChapter = await _chapterRepository.GetByBookIdAndOrderIndexAsync(
                    existingBook.Id, chapterDto.OrderIndex, cancellationToken);

                if (existingChapter == null)
                {
                    existingChapter = new ClassicChapter
                    {
                        Id = Guid.CreateVersion7(),
                        BookId = existingBook.Id,
                        Title = chapterDto.Title,
                        OrderIndex = chapterDto.OrderIndex,
                        TotalSentences = result.Sentences.Count(s => s.OrderIndex == chapterDto.OrderIndex),
                        IsPublished = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    await _chapterRepository.AddAsync(existingChapter, cancellationToken);
                }

                var chapterSentences = result.Sentences
                    .Where(s => s.OrderIndex == chapterDto.OrderIndex)
                    .OrderBy(s => s.OrderIndex)
                    .ToList();

                for (int i = 0; i < chapterSentences.Count; i++)
                {
                    var sentenceDto = chapterSentences[i];
                    var audioUrl = GetAudioUrl(audioDirectory, chapterDto.OrderIndex, i + 1);

                    var existingSentence = await _sentenceRepository.GetByChapterIdAndOrderIndexAsync(
                        existingChapter.Id, i + 1, cancellationToken);

                    if (existingSentence == null)
                    {
                        existingSentence = new ClassicSentence
                        {
                            Id = Guid.CreateVersion7(),
                            ChapterId = existingChapter.Id,
                            Content = sentenceDto.Content,
                            Pinyin = sentenceDto.Pinyin,
                            Note = sentenceDto.Note,
                            Translation = sentenceDto.Translation,
                            AudioUrl = audioUrl,
                            AudioFormat = "mp3",
                            AudioFileSize = 0,
                            AudioDuration = null,
                            OrderIndex = i + 1,
                            IsPublished = true,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        await _sentenceRepository.AddAsync(existingSentence, cancellationToken);
                    }
                }
            }

            result.Message = "数据导入成功";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"导入失败：{ex.Message}";
        }

        return result;
    }

    private ICrawlSource? CreateCrawler(string type, string sourceId)
    {
        return type.ToLower() switch
        {
            "html" => sourceId switch
            {
                "hancheng" => new HanChengCrawlSource(),
                _ => new HtmlCrawlSource()
            },
            _ => null
        };
    }

    private async Task DeleteExistingDataAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        var chapters = await _chapterRepository.GetByBookIdAsync(bookId, cancellationToken);
        foreach (var chapter in chapters)
        {
            await _chapterRepository.DeleteAsync(chapter, cancellationToken);
        }
    }

    private string GetAudioUrl(string audioDirectory, int chapterIndex, int sentenceIndex)
    {
        var fileName = $"{chapterIndex:D3}.mp3";
        return $"Files/{audioDirectory}/{fileName}";
    }
}
