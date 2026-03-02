using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data.Repositories;

/// <summary>
/// 经典书籍仓储实现
/// </summary>
public class ClassicBookRepository : EfCoreRepository<ClassicBook>, IClassicBookRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public ClassicBookRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// 根据分类获取书籍
    /// </summary>
    /// <param name="category">分类</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍列表</returns>
    public async Task<IEnumerable<ClassicBook>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicBooks
            .Where(b => b.Category == category && b.IsPublished)
            .OrderBy(b => b.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据难度等级获取书籍
    /// </summary>
    /// <param name="level">难度等级</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍列表</returns>
    public async Task<IEnumerable<ClassicBook>> GetByLevelAsync(byte level, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicBooks
            .Where(b => b.Level == level && b.IsPublished)
            .OrderBy(b => b.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 获取推荐书籍
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>推荐书籍列表</returns>
    public async Task<IEnumerable<ClassicBook>> GetRecommendedAsync(int limit, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicBooks
            .Where(b => b.IsPublished)
            .OrderByDescending(b => b.TotalSentences)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 搜索书籍
    /// </summary>
    /// <param name="keyword">关键词</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍列表</returns>
    public async Task<IEnumerable<ClassicBook>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return await _context.ClassicBooks
                .Where(b => b.IsPublished)
                .ToListAsync(cancellationToken);
        }

        return await _context.ClassicBooks
            .Where(b => b.IsPublished && 
                   (b.Title.Contains(keyword) || 
                    b.Author.Contains(keyword) || 
                    b.Description.Contains(keyword)))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 分页获取书籍
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="size">每页大小</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>分页结果</returns>
    public async Task<PagedResult<ClassicBook>> GetPagedAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.ClassicBooks.CountAsync(cancellationToken);
        var items = await _context.ClassicBooks
            .Where(b => b.IsPublished)
            .OrderBy(b => b.OrderIndex)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return new PagedResult<ClassicBook>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            Size = size
        };
    }

    /// <summary>
    /// 检查书籍是否存在
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicBooks.AnyAsync(b => b.Id == id, cancellationToken);
    }

    /// <summary>
    /// 根据标题获取书籍
    /// </summary>
    /// <param name="title">书名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍</returns>
    public async Task<ClassicBook?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicBooks
            .FirstOrDefaultAsync(b => b.Title == title, cancellationToken);
    }
}
