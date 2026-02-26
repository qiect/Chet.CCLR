using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Log;

namespace Chet.CCLR.WebApi.Contracts.IRepositories;

/// <summary>
/// 操作日志仓储接口
/// </summary>
public interface IOperationLogRepository : IRepository<OperationLog>
{
    /// <summary>
    /// 根据用户ID获取操作日志
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作日志列表</returns>
    Task<IEnumerable<OperationLog>> GetByUserIdAsync(Guid? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据操作类型获取日志
    /// </summary>
    /// <param name="operation">操作类型</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作日志列表</returns>
    Task<IEnumerable<OperationLog>> GetByOperationAsync(string operation, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据目标类型和目标ID获取日志
    /// </summary>
    /// <param name="targetType">目标类型</param>
    /// <param name="targetId">目标ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作日志列表</returns>
    Task<IEnumerable<OperationLog>> GetByTargetAsync(string targetType, string targetId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据日期范围获取日志
    /// </summary>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作日志列表</returns>
    Task<IEnumerable<OperationLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}