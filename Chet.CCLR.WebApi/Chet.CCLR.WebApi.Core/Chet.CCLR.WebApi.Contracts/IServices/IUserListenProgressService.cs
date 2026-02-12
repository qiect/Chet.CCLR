using Chet.CCLR.WebApi.DTOs.Request.Listen;
using Chet.CCLR.WebApi.DTOs.Response.Listen;
using Chet.CCLR.WebApi.DTOs.Response.Classic;

namespace Chet.CCLR.WebApi.Contracts.IServices;

/// <summary>
/// 用户听读进度服务接口
/// </summary>
public interface IUserListenProgressService
{
    /// <summary>
    /// 获取用户的书籍进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>进度信息</returns>
    Task<ProgressResponseDto?> GetUserProgressAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户所有书籍进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>进度列表</returns>
    Task<IEnumerable<ProgressResponseDto>> GetUserAllProgressAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新用户进度
    /// </summary>
    /// <param name="request">更新请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>更新后的进度</returns>
    Task<ProgressResponseDto> UpdateUserProgressAsync(UpdateProgressRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 重置用户书籍进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否重置成功</returns>
    Task<bool> ResetUserProgressAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户当前学习的句子
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>当前句子信息</returns>
    Task<SentenceResponseDto?> GetCurrentSentenceAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户学习统计
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>学习统计数据</returns>
    Task<LearningStatsResponseDto> GetUserLearningStatsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除用户进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeleteUserProgressAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default);
}