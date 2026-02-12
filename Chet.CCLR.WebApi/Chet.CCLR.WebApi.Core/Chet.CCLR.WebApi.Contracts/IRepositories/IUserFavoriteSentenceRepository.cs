using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Listen;

namespace Chet.CCLR.WebApi.Contracts.IRepositories;

/// <summary>
/// 用户收藏句子仓储接口
/// </summary>
public interface IUserFavoriteSentenceRepository : IRepository<UserFavoriteSentence>
{
    /// <summary>
    /// 根据用户ID获取收藏
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>收藏列表</returns>
    Task<IEnumerable<UserFavoriteSentence>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据用户ID和句子ID获取收藏
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>收藏信息</returns>
    Task<UserFavoriteSentence?> GetByUserIdAndSentenceIdAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查用户是否收藏了指定句子
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否收藏</returns>
    Task<bool> IsFavoritedAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除用户收藏
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作结果</returns>
    Task<bool> RemoveFavoriteAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default);
}