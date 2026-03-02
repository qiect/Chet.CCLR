using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;

namespace Chet.CCLR.WebApi.Contracts.IRepositories;

/// <summary>
/// 经典书籍仓储接口
/// </summary>
public interface IClassicBookRepository : IRepository<ClassicBook>
{
    /// <summary>
    /// 根据分类获取书籍
    /// </summary>
    /// <param name="category">分类</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍列表</returns>
    Task<IEnumerable<ClassicBook>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据难度等级获取书籍
    /// </summary>
    /// <param name="level">难度等级</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍列表</returns>
    Task<IEnumerable<ClassicBook>> GetByLevelAsync(byte level, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取推荐书籍
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>推荐书籍列表</returns>
    Task<IEnumerable<ClassicBook>> GetRecommendedAsync(int limit, CancellationToken cancellationToken = default);

    /// <summary>
    /// 搜索书籍
    /// </summary>
    /// <param name="keyword">关键词</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍列表</returns>
    Task<IEnumerable<ClassicBook>> SearchAsync(string keyword, CancellationToken cancellationToken = default);

    /// <summary>
    /// 分页获取书籍
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="size">每页大小</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>分页结果</returns>
    Task<PagedResult<ClassicBook>> GetPagedAsync(int page, int size, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查书籍是否存在
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据标题获取书籍
    /// </summary>
    /// <param name="title">书名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍</returns>
    Task<ClassicBook?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
}
