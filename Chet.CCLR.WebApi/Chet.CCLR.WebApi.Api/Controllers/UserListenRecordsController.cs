using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Listen;
using Microsoft.AspNetCore.Mvc;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 用户听读记录控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserListenRecordsController : ControllerBase
{
    private readonly IUserListenRecordService _recordService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="recordService">记录服务</param>
    public UserListenRecordsController(IUserListenRecordService recordService)
    {
        _recordService = recordService;
    }

    /// <summary>
    /// 获取用户指定日期的听读记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="date">日期</param>
    /// <returns>听读记录</returns>
    [HttpGet("user/{userId}/date/{date}")]
    public async Task<ActionResult<RecordResponseDto>> GetRecordByUserAndDate(string userId, DateOnly date)
    {
        var record = await _recordService.GetRecordByUserAndDateAsync(Guid.Parse(userId), date);
        if (record == null)
        {
            return NotFound();
        }
        return Ok(record);
    }

    /// <summary>
    /// 获取用户听读记录列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <returns>记录列表</returns>
    [HttpGet("user/{userId}/range")]
    public async Task<ActionResult<IEnumerable<RecordResponseDto>>> GetRecordsByUserAndDateRange(
        string userId, 
        [FromQuery] DateOnly startDate, 
        [FromQuery] DateOnly endDate)
    {
        var records = await _recordService.GetRecordsByUserAndDateRangeAsync(Guid.Parse(userId), startDate, endDate);
        return Ok(records);
    }

    /// <summary>
    /// 获取用户近期听读记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="days">天数</param>
    /// <returns>近期记录列表</returns>
    [HttpGet("user/{userId}/recent")]
    public async Task<ActionResult<IEnumerable<RecordResponseDto>>> GetRecentRecords(string userId, [FromQuery] int days = 7)
    {
        var records = await _recordService.GetRecentRecordsAsync(Guid.Parse(userId), days);
        return Ok(records);
    }

    /// <summary>
    /// 创建听读记录
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的记录</returns>
    [HttpPost]
    public async Task<ActionResult<RecordResponseDto>> CreateRecord([FromBody] CreateRecordRequestDto request)
    {
        var record = await _recordService.CreateRecordAsync(request);
        return CreatedAtAction(nameof(GetRecordByUserAndDate), new { userId = record.UserId, date = record.ListenDate }, record);
    }

    /// <summary>
    /// 更新听读记录
    /// </summary>
    /// <param name="id">记录ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新的记录</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<RecordResponseDto>> UpdateRecord(string id, [FromBody] UpdateRecordRequestDto request)
    {
        var record = await _recordService.UpdateRecordAsync(Guid.Parse(id), request);
        if (record == null)
        {
            return NotFound();
        }
        return Ok(record);
    }

    /// <summary>
    /// 获取用户连续听读天数
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>连续天数</returns>
    [HttpGet("user/{userId}/consecutive-days")]
    public async Task<ActionResult<int>> GetConsecutiveListenDays(string userId)
    {
        var days = await _recordService.GetConsecutiveListenDaysAsync(Guid.Parse(userId));
        return Ok(days);
    }

    /// <summary>
    /// 获取用户总听读天数
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>总听读天数</returns>
    [HttpGet("user/{userId}/total-days")]
    public async Task<ActionResult<int>> GetTotalListenDays(string userId)
    {
        var days = await _recordService.GetTotalListenDaysAsync(Guid.Parse(userId));
        return Ok(days);
    }

    /// <summary>
    /// 删除听读记录
    /// </summary>
    /// <param name="id">记录ID</param>
    /// <returns>删除结果</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecord(string id)
    {
        var result = await _recordService.DeleteRecordAsync(Guid.Parse(id));
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}