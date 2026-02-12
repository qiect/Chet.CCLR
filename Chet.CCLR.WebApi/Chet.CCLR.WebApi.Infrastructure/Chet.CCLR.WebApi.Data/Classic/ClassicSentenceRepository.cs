using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data.Repositories;

/// <summary>
/// 经典句子仓储实现
/// </summary>
public class ClassicSentenceRepository : EfCoreRepository<ClassicSentence>, IClassicSentenceRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public ClassicSentenceRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// 根据章节ID获取句子
    /// </summary>
    /// <param name="chapterId">章节ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子列表</returns>
    public async Task<IEnumerable<ClassicSentence>> GetByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicSentences
            .Where(s => s.ChapterId == chapterId)
            .OrderBy(s => s.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据章节ID和发布状态获取句子
    /// </summary>
    /// <param name="chapterId">章节ID</param>
    /// <param name="isPublished">是否发布</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子列表</returns>
    public async Task<IEnumerable<ClassicSentence>> GetByChapterIdAndPublishedAsync(Guid chapterId, bool isPublished, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicSentences
            .Where(s => s.ChapterId == chapterId && s.IsPublished == isPublished)
            .OrderBy(s => s.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据书籍ID获取句子
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子列表</returns>
    public async Task<IEnumerable<ClassicSentence>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicSentences
            .Where(s => s.Chapter.BookId == bookId)
            .OrderBy(s => s.Chapter.OrderIndex)
            .ThenBy(s => s.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 检查句子是否存在
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ClassicSentences.AnyAsync(s => s.Id == id, cancellationToken);
    }
}