using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Listen;
using Chet.CCLR.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 用户听读记录控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("提供用户听读记录管理相关的API接口，包括获取、创建、更新和删除听读记录")]
public class UserListenRecordsController : ControllerBase
{
    /// <summary>
    /// 记录服务，用于处理听读记录相关的业务逻辑
    /// </summary>
    private readonly IUserListenRecordService _recordService;

    /// <summary>
    /// 日志记录器，用于记录控制器操作日志
    /// </summary>
    private readonly ILogger<UserListenRecordsController> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="recordService">记录服务</param>
    public UserListenRecordsController(IUserListenRecordService recordService, ILogger<UserListenRecordsController> logger)
    {
        _recordService = recordService;
        _logger = logger;
    }

    /// <summary>
    /// 获取用户指定日期的听读记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="date">日期</param>
    /// <returns>听读记录</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserListenRecords/user/guid/date/2024-01-01
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "userId": "guid",
    ///             "listenDate": "2024-01-01",
    ///             "listenTime": 3600,
    ///             "sentencesCount": 10
    ///         },
    ///         "message": "Listen record retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回听读记录</response>
    /// <response code="404">记录不存在</response>
    [HttpGet("user/{userId}/date/{date}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecordByUserAndDate(string userId, DateOnly date)
    {
        _logger.LogInformation("Getting listen record for user {UserId} and date {Date}", userId, date);
        var record = await _recordService.GetRecordByUserAndDateAsync(Guid.Parse(userId), date);
        if (record == null)
        {
            return Ok(ApiResponse.Error("Listen record not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(record, "Listen record retrieved successfully"));
    }

    /// <summary>
    /// 获取用户听读记录列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <returns>记录列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserListenRecords/user/guid/range?startDate=2024-01-01&endDate=2024-01-31
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "userId": "guid",
    ///                 "listenDate": "2024-01-01",
    ///                 "listenTime": 3600,
    ///                 "sentencesCount": 10
    ///             }
    ///         ],
    ///         "message": "Listen records retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回记录列表</response>
    [HttpGet("user/{userId}/range")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecordsByUserAndDateRange(
        string userId, 
        [FromQuery] DateOnly startDate, 
        [FromQuery] DateOnly endDate)
    {
        _logger.LogInformation("Getting listen records for user {UserId} from {StartDate} to {EndDate}", userId, startDate, endDate);
        var records = await _recordService.GetRecordsByUserAndDateRangeAsync(Guid.Parse(userId), startDate, endDate);
        return Ok(ApiResponse.Ok(records, "Listen records retrieved successfully"));
    }

    /// <summary>
    /// 获取用户近期听读记录
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="days">天数</param>
    /// <returns>近期记录列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserListenRecords/user/guid/recent?days=7
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "userId": "guid",
    ///                 "listenDate": "2024-01-01",
    ///                 "listenTime": 3600,
    ///                 "sentencesCount": 10
    ///             }
    ///         ],
    ///         "message": "Recent listen records retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回近期记录列表</response>
    [HttpGet("user/{userId}/recent")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentRecords(string userId, [FromQuery] int days = 7)
    {
        _logger.LogInformation("Getting recent listen records for user {UserId} within {Days} days", userId, days);
        var records = await _recordService.GetRecentRecordsAsync(Guid.Parse(userId), days);
        return Ok(ApiResponse.Ok(records, "Recent listen records retrieved successfully"));
    }

    /// <summary>
    /// 创建听读记录
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的记录</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/UserListenRecords
    ///     {
    ///         "userId": "guid",
    ///         "listenDate": "2024-01-01",
    ///         "listenTime": 3600,
    ///         "sentencesCount": 10
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 201 Created
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "userId": "guid",
    ///             "listenDate": "2024-01-01",
    ///             "listenTime": 3600,
    ///             "sentencesCount": 10
    ///         },
    ///         "message": "Listen record created successfully",
    ///         "statusCode": 201
    ///     }
    /// </remarks>
    /// <response code="201">创建成功，返回创建的记录</response>
    /// <response code="400">创建失败，输入无效</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRecord([FromBody] CreateRecordRequestDto request)
    {
        _logger.LogInformation("Creating listen record for user {UserId} on date {Date}", request.UserId, request.ListenDate);
        var record = await _recordService.CreateRecordAsync(request);
        return CreatedAtAction(nameof(GetRecordByUserAndDate), new { userId = record.UserId, date = record.ListenDate }, ApiResponse.Ok(record, "Listen record created successfully", StatusCodes.Status201Created));
    }

    /// <summary>
    /// 更新听读记录
    /// </summary>
    /// <param name="id">记录ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新的记录</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     PUT /api/UserListenRecords/guid
    ///     {
    ///         "listenTime": 7200,
    ///         "sentencesCount": 20
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "userId": "guid",
    ///             "listenDate": "2024-01-01",
    ///             "listenTime": 7200,
    ///             "sentencesCount": 20
    ///         },
    ///         "message": "Listen record updated successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">更新成功，返回更新的记录</response>
    /// <response code="404">记录不存在</response>
    /// <response code="400">更新失败，输入无效</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRecord(string id, [FromBody] UpdateRecordRequestDto request)
    {
        _logger.LogInformation("Updating listen record with id: {Id}", id);
        var record = await _recordService.UpdateRecordAsync(Guid.Parse(id), request);
        if (record == null)
        {
            return Ok(ApiResponse.Error("Listen record not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(record, "Listen record updated successfully"));
    }

    /// <summary>
    /// 获取用户连续听读天数
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>连续天数</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserListenRecords/user/guid/consecutive-days
    ///     {
    ///         "success": true,
    ///         "data": 7,
    ///         "message": "Consecutive listen days retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回连续天数</response>
    [HttpGet("user/{userId}/consecutive-days")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConsecutiveListenDays(string userId)
    {
        _logger.LogInformation("Getting consecutive listen days for user: {UserId}", userId);
        var days = await _recordService.GetConsecutiveListenDaysAsync(Guid.Parse(userId));
        return Ok(ApiResponse.Ok(days, "Consecutive listen days retrieved successfully"));
    }

    /// <summary>
    /// 获取用户总听读天数
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>总听读天数</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserListenRecords/user/guid/total-days
    ///     {
    ///         "success": true,
    ///         "data": 100,
    ///         "message": "Total listen days retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回总听读天数</response>
    [HttpGet("user/{userId}/total-days")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalListenDays(string userId)
    {
        _logger.LogInformation("Getting total listen days for user: {UserId}", userId);
        var days = await _recordService.GetTotalListenDaysAsync(Guid.Parse(userId));
        return Ok(ApiResponse.Ok(days, "Total listen days retrieved successfully"));
    }

    /// <summary>
    /// 删除听读记录
    /// </summary>
    /// <param name="id">记录ID</param>
    /// <returns>删除结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     DELETE /api/UserListenRecords/guid
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "Listen record deleted successfully",
    ///         "statusCode": 204
    ///     }
    /// </remarks>
    /// <response code="204">删除成功</response>
    /// <response code="404">记录不存在</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRecord(string id)
    {
        _logger.LogInformation("Deleting listen record with id: {Id}", id);
        var result = await _recordService.DeleteRecordAsync(Guid.Parse(id));
        if (!result)
        {
            return Ok(ApiResponse.Error("Listen record not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.NoContent("Listen record deleted successfully"));
    }
}
