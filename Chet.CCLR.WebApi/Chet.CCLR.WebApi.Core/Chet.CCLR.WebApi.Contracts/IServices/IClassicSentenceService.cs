using Chet.CCLR.WebApi.DTOs.Request.Classic;
using Chet.CCLR.WebApi.DTOs.Response.Classic;

namespace Chet.CCLR.WebApi.Contracts.IServices;

/// <summary>
/// 经典句子服务接口
/// </summary>
public interface IClassicSentenceService
{
    /// <summary>
    /// 根据章节ID获取句子列表
    /// </summary>
    /// <param name="chapterId">章节ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子列表</returns>
    Task<IEnumerable<SentenceResponseDto>> GetSentencesByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据句子ID获取句子详情
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子详情</returns>
    Task<SentenceResponseDto?> GetSentenceByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取随机句子
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>随机句子列表</returns>
    Task<IEnumerable<SentenceResponseDto>> GetRandomSentencesAsync(int limit = 5, CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建新句子
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>创建的句子</returns>
    Task<SentenceResponseDto> CreateSentenceAsync(CreateSentenceRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新句子信息
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <param name="request">更新请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>更新的句子</returns>
    Task<SentenceResponseDto?> UpdateSentenceAsync(Guid id, UpdateSentenceRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除句子
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeleteSentenceAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 搜索句子内容
    /// </summary>
    /// <param name="keyword">关键词</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>搜索结果</returns>
    Task<IEnumerable<SentenceResponseDto>> SearchSentencesAsync(string keyword, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取句子总数
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>句子总数</returns>
    Task<int> GetTotalSentenceCountAsync(CancellationToken cancellationToken = default);
}