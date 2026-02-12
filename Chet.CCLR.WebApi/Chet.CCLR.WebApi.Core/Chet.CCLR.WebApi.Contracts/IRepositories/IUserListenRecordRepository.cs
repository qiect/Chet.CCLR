using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Listen;

namespace Chet.CCLR.WebApi.Contracts.IRepositories;

/// <summary>
/// 用户听读记录仓储接口
/// </summary>
public interface IUserListenRecordRepository : IRepository<UserListenRecord>
{
    /// <summary>
    /// 根据用户ID获取记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>记录列表</returns>
    Task<IEnumerable<UserListenRecord>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据用户ID和日期范围获取记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>记录列表</returns>
    Task<IEnumerable<UserListenRecord>> GetByUserIdAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据用户ID和日期获取记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="date">日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>记录信息</returns>
    Task<UserListenRecord?> GetByUserIdAndDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户有效听读天数
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>有效听读天数</returns>
    Task<int> GetUserValidListenDaysAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查用户指定日期的记录是否存在
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="date">日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsByUserIdAndDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default);
}