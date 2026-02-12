using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;

namespace Chet.CCLR.WebApi.Contracts.IRepositories;

/// <summary>
/// 经典章节仓储接口
/// </summary>
public interface IClassicChapterRepository : IRepository<ClassicChapter>
{
    /// <summary>
    /// 根据书籍ID获取章节
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>章节列表</returns>
    Task<IEnumerable<ClassicChapter>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据书籍ID和发布状态获取章节
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <param name="isPublished">是否发布</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>章节列表</returns>
    Task<IEnumerable<ClassicChapter>> GetByBookIdAndPublishedAsync(Guid bookId, bool isPublished, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查章节是否存在
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}