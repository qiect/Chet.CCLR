using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Listen;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data.Repositories;

/// <summary>
/// 用户听读记录仓储实现
/// </summary>
public class UserListenRecordRepository : EfCoreRepository<UserListenRecord>, IUserListenRecordRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public UserListenRecordRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// 根据用户ID获取记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>记录列表</returns>
    public async Task<IEnumerable<UserListenRecord>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserListenRecords
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.ListenDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据用户ID和日期范围获取记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>记录列表</returns>
    public async Task<IEnumerable<UserListenRecord>> GetByUserIdAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _context.UserListenRecords
            .Where(r => r.UserId == userId && r.ListenDate >= startDate && r.ListenDate <= endDate)
            .OrderBy(r => r.ListenDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据用户ID和日期获取记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="date">日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>记录信息</returns>
    public async Task<UserListenRecord?> GetByUserIdAndDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default)
    {
        return await _context.UserListenRecords
            .FirstOrDefaultAsync(r => r.UserId == userId && r.ListenDate == date, cancellationToken);
    }

    /// <summary>
    /// 获取用户有效听读天数
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>有效听读天数</returns>
    public async Task<int> GetUserValidListenDaysAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserListenRecords
            .Where(r => r.UserId == userId && r.IsValidDay)
            .CountAsync(cancellationToken);
    }

    /// <summary>
    /// 检查用户指定日期的记录是否存在
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="date">日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    public async Task<bool> ExistsByUserIdAndDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default)
    {
        return await _context.UserListenRecords.AnyAsync(r => r.UserId == userId && r.ListenDate == date, cancellationToken);
    }
}