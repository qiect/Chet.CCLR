using Chet.CCLR.WebApi.DTOs.Request.Classic;
using Chet.CCLR.WebApi.DTOs.Response.Classic;

namespace Chet.CCLR.WebApi.Contracts.IServices;

/// <summary>
/// 经典章节服务接口
/// </summary>
public interface IClassicChapterService
{
    /// <summary>
    /// 根据书籍ID获取章节列表
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>章节列表</returns>
    Task<IEnumerable<ChapterResponseDto>> GetChaptersByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据章节ID获取章节详情
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>章节详情</returns>
    Task<ChapterResponseDto?> GetChapterByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取带句子的章节详情
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>章节详情包含句子</returns>
    Task<ChapterWithSentencesResponseDto?> GetChapterWithSentencesAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建新章节
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>创建的章节</returns>
    Task<ChapterResponseDto> CreateChapterAsync(CreateChapterRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新章节信息
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <param name="request">更新请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>更新的章节</returns>
    Task<ChapterResponseDto?> UpdateChapterAsync(Guid id, UpdateChapterRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除章节
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeleteChapterAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取章节的句子数量
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子数量</returns>
    Task<int> GetSentenceCountByChapterIdAsync(Guid id, CancellationToken cancellationToken = default);
}