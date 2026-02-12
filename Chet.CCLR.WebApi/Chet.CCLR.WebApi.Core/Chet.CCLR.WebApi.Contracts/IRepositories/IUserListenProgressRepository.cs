using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Listen;

namespace Chet.CCLR.WebApi.Contracts.IRepositories;

/// <summary>
/// 用户听读进度仓储接口
/// </summary>
public interface IUserListenProgressRepository : IRepository<UserListenProgress>
{
    /// <summary>
    /// 根据用户ID获取进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>进度列表</returns>
    Task<IEnumerable<UserListenProgress>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据用户ID和书籍ID获取进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>进度信息</returns>
    Task<UserListenProgress?> GetByUserIdAndBookIdAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查用户书籍进度是否存在
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsByUserIdAndBookIdAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default);
}