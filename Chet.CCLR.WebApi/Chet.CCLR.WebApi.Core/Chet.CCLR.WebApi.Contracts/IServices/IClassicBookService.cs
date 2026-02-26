using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.DTOs.Classic;

namespace Chet.CCLR.WebApi.Contracts.IServices;

/// <summary>
/// 经典书籍服务接口
/// </summary>
public interface IClassicBookService
{
    /// <summary>
    /// 获取所有经典书籍
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍列表</returns>
    Task<IEnumerable<BookResponseDto>> GetAllBooksAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据ID获取书籍详情
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍详情</returns>
    Task<BookResponseDto?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据分类获取书籍
    /// </summary>
    /// <param name="category">分类</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>书籍列表</returns>
    Task<IEnumerable<BookResponseDto>> GetBooksByCategoryAsync(string category, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取推荐书籍
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>推荐书籍列表</returns>
    Task<IEnumerable<BookResponseDto>> GetRecommendedBooksAsync(int limit = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// 搜索书籍
    /// </summary>
    /// <param name="keyword">关键词</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>搜索结果</returns>
    Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string keyword, CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建新书籍
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>创建的书籍</returns>
    Task<BookResponseDto> CreateBookAsync(CreateBookRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新书籍信息
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <param name="request">更新请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>更新的书籍</returns>
    Task<BookResponseDto?> UpdateBookAsync(Guid id, UpdateBookRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除书籍
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeleteBookAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取分页书籍列表
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="size">每页大小</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>分页结果</returns>
    Task<PagedResult<BookResponseDto>> GetPagedBooksAsync(int page, int size, CancellationToken cancellationToken = default);
}