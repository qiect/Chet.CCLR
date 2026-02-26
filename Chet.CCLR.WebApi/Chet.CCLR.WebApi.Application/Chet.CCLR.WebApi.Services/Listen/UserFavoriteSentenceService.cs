using AutoMapper;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Domain.Listen;
using Chet.CCLR.WebApi.DTOs.Listen;

namespace Chet.CCLR.WebApi.Services.Listen;

/// <summary>
/// 用户收藏句子服务实现
/// </summary>
public class UserFavoriteSentenceService : IUserFavoriteSentenceService
{
    private readonly IUserFavoriteSentenceRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="repository">收藏仓储</param>
    /// <param name="mapper">对象映射器</param>
    public UserFavoriteSentenceService(IUserFavoriteSentenceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FavoriteResponseDto>> GetUserFavoritesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var favorites = await _repository.GetByUserIdAsync(userId, cancellationToken);
        return _mapper.Map<IEnumerable<FavoriteResponseDto>>(favorites);
    }

    /// <inheritdoc />
    public async Task<bool> IsFavoritedAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default)
    {
        return await _repository.IsFavoritedAsync(userId, sentenceId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<FavoriteResponseDto> AddFavoriteAsync(AddFavoriteRequestDto request, CancellationToken cancellationToken = default)
    {
        // 检查是否已收藏
        var alreadyFavorited = await _repository.IsFavoritedAsync(Guid.Parse(request.UserId), Guid.Parse(request.SentenceId), cancellationToken);
        if (alreadyFavorited)
        {
            throw new InvalidOperationException($"Sentence {request.SentenceId} is already favorited by user {request.UserId}");
        }

        var favorite = _mapper.Map<UserFavoriteSentence>(request);
        favorite.Id = Guid.CreateVersion7();
        favorite.UserId = Guid.Parse(request.UserId);
        favorite.SentenceId = Guid.Parse(request.SentenceId);
        favorite.Note = request.Note;
        favorite.IsPublic = request.IsPublic;
        await _repository.AddAsync(favorite, cancellationToken);
        return _mapper.Map<FavoriteResponseDto>(favorite);
    }

    /// <inheritdoc />
    public async Task<bool> RemoveFavoriteAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default)
    {
        return await _repository.RemoveFavoriteAsync(userId, sentenceId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<FavoriteResponseDto?> GetFavoriteAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default)
    {
        var favorite = await _repository.GetByUserIdAndSentenceIdAsync(userId, sentenceId, cancellationToken);
        return _mapper.Map<FavoriteResponseDto>(favorite);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FavoriteResponseDto>> GetPopularFavoritesAsync(int limit = 10, CancellationToken cancellationToken = default)
    {
        // 实际实现中需要根据收藏次数或其他指标获取热门收藏
        // 暂时返回最近收藏的项目
        var favorites = await _repository.GetAllAsync(cancellationToken);
        var popular = favorites.OrderByDescending(f => f.CreatedAt).Take(limit);
        return _mapper.Map<IEnumerable<FavoriteResponseDto>>(popular);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateFavoriteNoteAsync(Guid userId, Guid sentenceId, string note, CancellationToken cancellationToken = default)
    {
        var favorite = await _repository.GetByUserIdAndSentenceIdAsync(userId, sentenceId, cancellationToken);
        if (favorite == null)
        {
            return false;
        }

        favorite.Note = note;
        favorite.UpdatedAt = DateTime.Now;
        await _repository.UpdateAsync(favorite, cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<FavoriteStatsResponseDto> GetUserFavoriteStatsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var favorites = await _repository.GetByUserIdAsync(userId, cancellationToken);
        var count = favorites.Count();
        
        // TODO: 实现更详细的统计逻辑，如书籍数量、章节数量等
        // 这里需要从收藏的句子中关联到书籍和章节
        
        return new FavoriteStatsResponseDto
        {
            UserId = userId.ToString(),
            TotalFavorites = count,
            BooksCount = 0,
            ChaptersCount = 0,
            LastUpdated = favorites.Any() ? favorites.Max(f => f.UpdatedAt) : null,
            TopFavorites = new List<string>() // 暂时返回空列表
        };
    }
}