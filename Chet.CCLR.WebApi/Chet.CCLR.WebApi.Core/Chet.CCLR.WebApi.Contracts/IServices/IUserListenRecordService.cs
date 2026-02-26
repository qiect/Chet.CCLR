using Chet.CCLR.WebApi.DTOs.Listen;

namespace Chet.CCLR.WebApi.Contracts.IServices;

/// <summary>
/// 用户听读记录服务接口
/// </summary>
public interface IUserListenRecordService
{
    /// <summary>
    /// 获取用户指定日期的听读记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="date">日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>听读记录</returns>
    Task<RecordResponseDto?> GetRecordByUserAndDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户听读记录列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>记录列表</returns>
    Task<IEnumerable<RecordResponseDto>> GetRecordsByUserAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户近期听读记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="days">天数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>近期记录列表</returns>
    Task<IEnumerable<RecordResponseDto>> GetRecentRecordsAsync(Guid userId, int days = 7, CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建听读记录
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>创建的记录</returns>
    Task<RecordResponseDto> CreateRecordAsync(CreateRecordRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新听读记录
    /// </summary>
    /// <param name="id">记录ID</param>
    /// <param name="request">更新请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>更新的记录</returns>
    Task<RecordResponseDto?> UpdateRecordAsync(Guid id, UpdateRecordRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户连续听读天数
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>连续天数</returns>
    Task<int> GetConsecutiveListenDaysAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户总听读天数
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>总听读天数</returns>
    Task<int> GetTotalListenDaysAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除听读记录
    /// </summary>
    /// <param name="id">记录ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeleteRecordAsync(Guid id, CancellationToken cancellationToken = default);
}