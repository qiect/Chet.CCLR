using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Listen;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data.Repositories;

/// <summary>
/// 用户收藏句子仓储实现
/// </summary>
public class UserFavoriteSentenceRepository : EfCoreRepository<UserFavoriteSentence>, IUserFavoriteSentenceRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public UserFavoriteSentenceRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// 根据用户ID获取收藏
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>收藏列表</returns>
    public async Task<IEnumerable<UserFavoriteSentence>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserFavoriteSentences
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据用户ID和句子ID获取收藏
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>收藏信息</returns>
    public async Task<UserFavoriteSentence?> GetByUserIdAndSentenceIdAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default)
    {
        return await _context.UserFavoriteSentences
            .FirstOrDefaultAsync(f => f.UserId == userId && f.SentenceId == sentenceId, cancellationToken);
    }

    /// <summary>
    /// 检查用户是否收藏了指定句子
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否收藏</returns>
    public async Task<bool> IsFavoritedAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default)
    {
        return await _context.UserFavoriteSentences.AnyAsync(f => f.UserId == userId && f.SentenceId == sentenceId, cancellationToken);
    }

    /// <summary>
    /// 删除用户收藏
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作结果</returns>
    public async Task<bool> RemoveFavoriteAsync(Guid userId, Guid sentenceId, CancellationToken cancellationToken = default)
    {
        var favorite = await _context.UserFavoriteSentences
            .FirstOrDefaultAsync(f => f.UserId == userId && f.SentenceId == sentenceId, cancellationToken);

        if (favorite != null)
        {
            _context.UserFavoriteSentences.Remove(favorite);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }
}