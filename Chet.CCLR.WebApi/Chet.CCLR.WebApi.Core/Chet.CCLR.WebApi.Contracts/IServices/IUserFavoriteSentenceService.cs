using Chet.CCLR.WebApi.DTOs.Request.Listen;
using Chet.CCLR.WebApi.DTOs.Response.Listen;

namespace Chet.CCLR.WebApi.Contracts.IServices;

/// <summary>
/// 用户收藏句子服务接口
/// </summary>
public interface IUserFavoriteSentenceService
{
    /// <summary>
    /// 获取用户的收藏列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>收藏列表</returns>
    Task<IEnumerable<FavoriteResponseDto>> GetUserFavoritesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户是否收藏某句子
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否收藏</returns>
    Task<bool> IsFavoritedAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 添加收藏
    /// </summary>
    /// <param name="request">收藏请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>收藏结果</returns>
    Task<FavoriteResponseDto> AddFavoriteAsync(AddFavoriteRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除收藏
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否删除成功</returns>
    Task<bool> RemoveFavoriteAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取收藏详情
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>收藏详情</returns>
    Task<FavoriteResponseDto?> GetFavoriteAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取热门收藏句子
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>热门收藏列表</returns>
    Task<IEnumerable<FavoriteResponseDto>> GetPopularFavoritesAsync(int limit = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新收藏备注
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="note">备注</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否更新成功</returns>
    Task<bool> UpdateFavoriteNoteAsync(Guid userId, Guid sentenceId, string note, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户收藏统计
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>收藏统计</returns>
    Task<FavoriteStatsResponseDto> GetUserFavoriteStatsAsync(Guid userId, CancellationToken cancellationToken = default);
}