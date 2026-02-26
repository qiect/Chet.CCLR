using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.DTOs.Classic;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;
using AutoMapper;

namespace Chet.CCLR.WebApi.Services.Classic;

/// <summary>
/// 经典句子服务实现
/// </summary>
public class ClassicSentenceService : IClassicSentenceService
{
    private readonly IClassicSentenceRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="repository">句子仓储</param>
    /// <param name="mapper">对象映射器</param>
    public ClassicSentenceService(IClassicSentenceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SentenceResponseDto>> GetSentencesByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default)
    {
        var sentences = await _repository.GetByChapterIdAsync(chapterId, cancellationToken);
        return _mapper.Map<IEnumerable<SentenceResponseDto>>(sentences);
    }

    /// <inheritdoc />
    public async Task<SentenceResponseDto?> GetSentenceByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sentence = await _repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<SentenceResponseDto>(sentence);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SentenceResponseDto>> GetRandomSentencesAsync(int limit = 5, CancellationToken cancellationToken = default)
    {
        var sentences = await _repository.GetAllAsync(cancellationToken);
        var random = new Random();
        var shuffled = sentences.OrderBy(x => random.Next());
        return _mapper.Map<IEnumerable<SentenceResponseDto>>(shuffled.Take(limit));
    }

    /// <inheritdoc />
    public async Task<SentenceResponseDto> CreateSentenceAsync(CreateSentenceRequestDto request, CancellationToken cancellationToken = default)
    {
        var sentence = _mapper.Map<ClassicSentence>(request);
        sentence.Id = Guid.CreateVersion7();
        await _repository.AddAsync(sentence, cancellationToken);
        return _mapper.Map<SentenceResponseDto>(sentence);
    }

    /// <inheritdoc />
    public async Task<SentenceResponseDto?> UpdateSentenceAsync(Guid id, UpdateSentenceRequestDto request, CancellationToken cancellationToken = default)
    {
        var existingSentence = await _repository.GetByIdAsync(id, cancellationToken);
        if (existingSentence == null)
        {
            return null;
        }

        _mapper.Map(request, existingSentence); // 将请求数据映射到现有实体
        await _repository.UpdateAsync(existingSentence, cancellationToken);
        return _mapper.Map<SentenceResponseDto>(existingSentence);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteSentenceAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            return false;
        }

        var sentence = await _repository.GetByIdAsync(id, cancellationToken);
        if (sentence == null)
        {
            return false; // Should not happen if exists returned true
        }

        await _repository.DeleteAsync(sentence, cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SentenceResponseDto>> SearchSentencesAsync(string keyword, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            var allSentences = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<SentenceResponseDto>>(allSentences);
        }

        // 在实际实现中，这里应该查询数据库
        // 由于仓储接口中没有搜索方法，我们暂时获取所有句子并进行过滤
        var allSentencesForFiltering = await _repository.GetAllAsync(cancellationToken);
        var filtered = allSentencesForFiltering.Where(s => 
            s.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            (s.Pinyin?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (s.Translation?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (s.Note?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
        );

        return _mapper.Map<IEnumerable<SentenceResponseDto>>(filtered);
    }

    /// <inheritdoc />
    public async Task<int> GetTotalSentenceCountAsync(CancellationToken cancellationToken = default)
    {
        var sentences = await _repository.GetAllAsync(cancellationToken);
        return sentences.Count();
    }
}