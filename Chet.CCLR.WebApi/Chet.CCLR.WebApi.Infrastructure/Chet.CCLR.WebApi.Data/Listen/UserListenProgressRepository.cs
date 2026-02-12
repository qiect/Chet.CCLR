using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Listen;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data.Repositories;

/// <summary>
/// 用户听读进度仓储实现
/// </summary>
public class UserListenProgressRepository : EfCoreRepository<UserListenProgress>, IUserListenProgressRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public UserListenProgressRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// 根据用户ID获取进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>进度列表</returns>
    public async Task<IEnumerable<UserListenProgress>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserListenProgress
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据用户ID和书籍ID获取进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>进度信息</returns>
    public async Task<UserListenProgress?> GetByUserIdAndBookIdAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        return await _context.UserListenProgress
            .FirstOrDefaultAsync(p => p.UserId == userId && p.BookId == bookId, cancellationToken);
    }

    /// <summary>
    /// 检查用户书籍进度是否存在
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    public async Task<bool> ExistsByUserIdAndBookIdAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        return await _context.UserListenProgress.AnyAsync(p => p.UserId == userId && p.BookId == bookId, cancellationToken);
    }
}