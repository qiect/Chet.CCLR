using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Classic;
using Chet.CCLR.WebApi.DTOs.Listen;
using Chet.CCLR.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 用户听读进度控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("提供用户听读进度管理相关的API接口，包括获取、更新和删除听读进度")]
public class UserListenProgressController : ControllerBase
{
    /// <summary>
    /// 进度服务，用于处理听读进度相关的业务逻辑
    /// </summary>
    private readonly IUserListenProgressService _progressService;

    /// <summary>
    /// 日志记录器，用于记录控制器操作日志
    /// </summary>
    private readonly ILogger<UserListenProgressController> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="progressService">进度服务</param>
    public UserListenProgressController(IUserListenProgressService progressService, ILogger<UserListenProgressController> logger)
    {
        _progressService = progressService;
        _logger = logger;
    }

    /// <summary>
    /// 获取用户的书籍进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <returns>进度信息</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserListenProgress/user/guid/book/guid
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "userId": "guid",
    ///             "bookId": "guid",
    ///             "currentChapterId": "guid",
    ///             "currentSentenceId": "guid",
    ///             "progressPercentage": 50.5
    ///         },
    ///         "message": "User progress retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回进度信息</response>
    /// <response code="404">进度不存在</response>
    [HttpGet("user/{userId}/book/{bookId}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserProgress(string userId, string bookId)
    {
        _logger.LogInformation("Getting user progress for user {UserId} and book {BookId}", userId, bookId);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid user ID", StatusCodes.Status400BadRequest));
        }
        if (string.IsNullOrWhiteSpace(bookId) || !Guid.TryParse(bookId, out var bookIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid book ID", StatusCodes.Status400BadRequest));
        }
        var progress = await _progressService.GetUserProgressAsync(userIdGuid, bookIdGuid);
        if (progress == null)
        {
            return Ok(ApiResponse.Error("User progress not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(progress, "User progress retrieved successfully"));
    }

    /// <summary>
    /// 获取用户所有书籍进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>进度列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserListenProgress/user/guid/all
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "userId": "guid",
    ///                 "bookId": "guid",
    ///                 "currentChapterId": "guid",
    ///                 "currentSentenceId": "guid",
    ///                 "progressPercentage": 50.5
    ///             }
    ///         ],
    ///         "message": "User all progress retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回进度列表</response>
    [HttpGet("user/{userId}/all")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserAllProgress(string userId)
    {
        _logger.LogInformation("Getting all progress for user: {UserId}", userId);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid user ID", StatusCodes.Status400BadRequest));
        }
        var progresses = await _progressService.GetUserAllProgressAsync(userIdGuid);
        return Ok(ApiResponse.Ok(progresses, "User all progress retrieved successfully"));
    }

    /// <summary>
    /// 更新用户进度
    /// </summary>
    /// <param name="request">更新请求</param>
    /// <returns>更新后的进度</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/UserListenProgress
    ///     {
    ///         "userId": "guid",
    ///         "bookId": "guid",
    ///         "currentChapterId": "guid",
    ///         "currentSentenceId": "guid",
    ///         "progressPercentage": 60.0
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
    ///             "bookId": "guid",
    ///             "currentChapterId": "guid",
    ///             "currentSentenceId": "guid",
    ///             "progressPercentage": 60.0
    ///         },
    ///         "message": "User progress updated successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">更新成功，返回更新后的进度</response>
    /// <response code="400">更新失败，输入无效</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserProgress([FromBody] UpdateProgressRequestDto request)
    {
        _logger.LogInformation("Updating user progress for user {UserId} and book {BookId}", request.UserId, request.BookId);
        var progress = await _progressService.UpdateUserProgressAsync(request);
        return Ok(ApiResponse.Ok(progress, "User progress updated successfully"));
    }

    /// <summary>
    /// 重置用户书籍进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <returns>重置结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     POST /api/UserListenProgress/reset/user/guid/book/guid
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "User progress reset successfully",
    ///         "statusCode": 204
    ///     }
    /// </remarks>
    /// <response code="204">重置成功</response>
    /// <response code="404">进度不存在</response>
    [HttpPost("reset/user/{userId}/book/{bookId}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetUserProgress(string userId, string bookId)
    {
        _logger.LogInformation("Resetting user progress for user {UserId} and book {BookId}", userId, bookId);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid user ID", StatusCodes.Status400BadRequest));
        }
        if (string.IsNullOrWhiteSpace(bookId) || !Guid.TryParse(bookId, out var bookIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid book ID", StatusCodes.Status400BadRequest));
        }
        var result = await _progressService.ResetUserProgressAsync(userIdGuid, bookIdGuid);
        if (!result)
        {
            return Ok(ApiResponse.Error("User progress not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.NoContent("User progress reset successfully"));
    }

    /// <summary>
    /// 获取用户当前学习的句子
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <returns>当前句子信息</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserListenProgress/user/guid/book/guid/current-sentence
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "chapterId": "guid",
    ///             "content": "句子内容",
    ///             "translation": "翻译"
    ///         },
    ///         "message": "Current sentence retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回当前句子信息</response>
    /// <response code="404">当前句子不存在</response>
    [HttpGet("user/{userId}/book/{bookId}/current-sentence")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentSentence(string userId, string bookId)
    {
        _logger.LogInformation("Getting current sentence for user {UserId} and book {BookId}", userId, bookId);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid user ID", StatusCodes.Status400BadRequest));
        }
        if (string.IsNullOrWhiteSpace(bookId) || !Guid.TryParse(bookId, out var bookIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid book ID", StatusCodes.Status400BadRequest));
        }
        var sentence = await _progressService.GetCurrentSentenceAsync(userIdGuid, bookIdGuid);
        if (sentence == null)
        {
            return Ok(ApiResponse.Error("Current sentence not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(sentence, "Current sentence retrieved successfully"));
    }

    /// <summary>
    /// 获取用户学习统计
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>学习统计数据</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserListenProgress/user/guid/stats
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "totalBooks": 5,
    ///             "totalChapters": 50,
    ///             "totalSentences": 500,
    ///             "totalListenTime": 3600
    ///         },
    ///         "message": "User learning stats retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回学习统计数据</response>
    [HttpGet("user/{userId}/stats")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserLearningStats(string userId)
    {
        _logger.LogInformation("Getting user learning stats for user: {UserId}", userId);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid user ID", StatusCodes.Status400BadRequest));
        }
        var stats = await _progressService.GetUserLearningStatsAsync(userIdGuid);
        return Ok(ApiResponse.Ok(stats, "User learning stats retrieved successfully"));
    }

    /// <summary>
    /// 删除用户进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <returns>删除结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     DELETE /api/UserListenProgress/user/guid/book/guid
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "User progress deleted successfully",
    ///         "statusCode": 204
    ///     }
    /// </remarks>
    /// <response code="204">删除成功</response>
    /// <response code="404">进度不存在</response>
    [HttpDelete("user/{userId}/book/{bookId}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserProgress(string userId, string bookId)
    {
        _logger.LogInformation("Deleting user progress for user {UserId} and book {BookId}", userId, bookId);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid user ID", StatusCodes.Status400BadRequest));
        }
        if (string.IsNullOrWhiteSpace(bookId) || !Guid.TryParse(bookId, out var bookIdGuid))
        {
            return Ok(ApiResponse.Error("Invalid book ID", StatusCodes.Status400BadRequest));
        }
        var result = await _progressService.DeleteUserProgressAsync(userIdGuid, bookIdGuid);
        if (!result)
        {
            return Ok(ApiResponse.Error("User progress not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.NoContent("User progress deleted successfully"));
    }
}
