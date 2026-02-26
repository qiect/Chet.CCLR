using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Log;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data.Repositories;

/// <summary>
/// 操作日志仓储实现
/// </summary>
public class OperationLogRepository : EfCoreRepository<OperationLog>, IOperationLogRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public OperationLogRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// 根据用户ID获取操作日志
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作日志列表</returns>
    public async Task<IEnumerable<OperationLog>> GetByUserIdAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        if (!userId.HasValue)
        {
            return await _context.OperationLogs
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        return await _context.OperationLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据操作类型获取日志
    /// </summary>
    /// <param name="operation">操作类型</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作日志列表</returns>
    public async Task<IEnumerable<OperationLog>> GetByOperationAsync(string operation, CancellationToken cancellationToken = default)
    {
        return await _context.OperationLogs
            .Where(l => l.Operation == operation)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据目标类型和目标ID获取日志
    /// </summary>
    /// <param name="targetType">目标类型</param>
    /// <param name="targetId">目标ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作日志列表</returns>
    public async Task<IEnumerable<OperationLog>> GetByTargetAsync(string targetType, string targetId, CancellationToken cancellationToken = default)
    {
        return await _context.OperationLogs
            .Where(l => l.TargetType == targetType && l.TargetId == targetId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据日期范围获取日志
    /// </summary>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作日志列表</returns>
    public async Task<IEnumerable<OperationLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.OperationLogs
            .Where(l => l.CreatedAt >= startDate && l.CreatedAt <= endDate)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}