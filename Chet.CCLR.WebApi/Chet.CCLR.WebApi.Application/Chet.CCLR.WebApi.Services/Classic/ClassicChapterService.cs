using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.DTOs.Classic;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;
using AutoMapper;

namespace Chet.CCLR.WebApi.Services.Classic;

/// <summary>
/// 经典章节服务实现
/// </summary>
public class ClassicChapterService : IClassicChapterService
{
    private readonly IClassicChapterRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="repository">章节仓储</param>
    /// <param name="mapper">对象映射器</param>
    public ClassicChapterService(IClassicChapterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ChapterResponseDto>> GetChaptersByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        var chapters = await _repository.GetByBookIdAsync(bookId, cancellationToken);
        return _mapper.Map<IEnumerable<ChapterResponseDto>>(chapters);
    }

    /// <inheritdoc />
    public async Task<ChapterResponseDto?> GetChapterByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var chapter = await _repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<ChapterResponseDto>(chapter);
    }

    /// <inheritdoc />
    public async Task<ChapterWithSentencesResponseDto?> GetChapterWithSentencesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var chapter = await _repository.GetByIdAsync(id, cancellationToken);
        if (chapter == null)
        {
            return null;
        }

        var sentences = await _repository.GetByBookIdAndPublishedAsync(chapter.BookId, true, cancellationToken);
        var chapterDto = _mapper.Map<ChapterResponseDto>(chapter);
        var sentencesDto = _mapper.Map<IEnumerable<SentenceResponseDto>>(sentences);

        return new ChapterWithSentencesResponseDto
        {
            Chapter = chapterDto,
            Sentences = sentencesDto
        };
    }

    /// <inheritdoc />
    public async Task<ChapterResponseDto> CreateChapterAsync(CreateChapterRequestDto request, CancellationToken cancellationToken = default)
    {
        var chapter = _mapper.Map<ClassicChapter>(request);
        chapter.Id = Guid.CreateVersion7();
        await _repository.AddAsync(chapter, cancellationToken);
        return _mapper.Map<ChapterResponseDto>(chapter);
    }

    /// <inheritdoc />
    public async Task<ChapterResponseDto?> UpdateChapterAsync(Guid id, UpdateChapterRequestDto request, CancellationToken cancellationToken = default)
    {
        var existingChapter = await _repository.GetByIdAsync(id, cancellationToken);
        if (existingChapter == null)
        {
            return null;
        }

        _mapper.Map(request, existingChapter); // 将请求数据映射到现有实体
        await _repository.UpdateAsync(existingChapter, cancellationToken);
        return _mapper.Map<ChapterResponseDto>(existingChapter);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteChapterAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            return false;
        }

        var chapter = await _repository.GetByIdAsync(id, cancellationToken);
        if (chapter == null)
        {
            return false; // Should not happen if exists returned true
        }

        await _repository.DeleteAsync(chapter, cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<int> GetSentenceCountByChapterIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var chapter = await _repository.GetByIdAsync(id, cancellationToken);
        return chapter?.TotalSentences ?? 0;
    }
}