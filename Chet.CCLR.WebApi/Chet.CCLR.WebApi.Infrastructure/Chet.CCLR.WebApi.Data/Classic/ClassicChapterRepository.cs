using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data.Repositories;

/// <summary>
/// 经典章节仓储实现
/// </summary>
public class ClassicChapterRepository : EfCoreRepository<ClassicChapter>, IClassicChapterRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public ClassicChapterRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// 根据书籍ID获取章节
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>章节列表</returns>
    public async Task<IEnumerable<ClassicChapter>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicChapters
            .Where(c => c.BookId == bookId)
            .OrderBy(c => c.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据书籍ID和发布状态获取章节
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <param name="isPublished">是否发布</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>章节列表</returns>
    public async Task<IEnumerable<ClassicChapter>> GetByBookIdAndPublishedAsync(Guid bookId, bool isPublished, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicChapters
            .Where(c => c.BookId == bookId && c.IsPublished == isPublished)
            .OrderBy(c => c.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 检查章节是否存在
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicChapters.AnyAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// 根据书籍ID和章节序号获取章节
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <param name="orderIndex">章节序号</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>章节实体</returns>
    public async Task<ClassicChapter?> GetByBookIdAndOrderIndexAsync(Guid bookId, int orderIndex, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicChapters
            .FirstOrDefaultAsync(c => c.BookId == bookId && c.OrderIndex == orderIndex, cancellationToken);
    }
}