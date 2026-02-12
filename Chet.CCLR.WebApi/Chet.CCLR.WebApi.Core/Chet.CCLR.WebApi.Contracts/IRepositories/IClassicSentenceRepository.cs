using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;

namespace Chet.CCLR.WebApi.Contracts.IRepositories;

/// <summary>
/// 经典句子仓储接口
/// </summary>
public interface IClassicSentenceRepository : IRepository<ClassicSentence>
{
    /// <summary>
    /// 根据章节ID获取句子
    /// </summary>
    /// <param name="chapterId">章节ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子列表</returns>
    Task<IEnumerable<ClassicSentence>> GetByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据章节ID和发布状态获取句子
    /// </summary>
    /// <param name="chapterId">章节ID</param>
    /// <param name="isPublished">是否发布</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子列表</returns>
    Task<IEnumerable<ClassicSentence>> GetByChapterIdAndPublishedAsync(Guid chapterId, bool isPublished, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据书籍ID获取句子
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子列表</returns>
    Task<IEnumerable<ClassicSentence>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查句子是否存在
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}