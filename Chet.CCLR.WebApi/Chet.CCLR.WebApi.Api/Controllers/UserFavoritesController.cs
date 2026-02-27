using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Listen;
using Chet.CCLR.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 用户收藏控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("提供用户收藏管理相关的API接口，包括获取、添加和删除收藏")]
public class UserFavoritesController : ControllerBase
{
    /// <summary>
    /// 收藏服务，用于处理收藏相关的业务逻辑
    /// </summary>
    private readonly IUserFavoriteSentenceService _favoriteService;

    /// <summary>
    /// 日志记录器，用于记录控制器操作日志
    /// </summary>
    private readonly ILogger<UserFavoritesController> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="favoriteService">收藏服务</param>
    public UserFavoritesController(IUserFavoriteSentenceService favoriteService, ILogger<UserFavoritesController> logger)
    {
        _favoriteService = favoriteService;
        _logger = logger;
    }

    /// <summary>
    /// 获取用户的收藏列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>收藏列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserFavorites/user/guid
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "userId": "guid",
    ///                 "sentenceId": "guid",
    ///                 "note": "备注"
    ///             }
    ///         ],
    ///         "message": "User favorites retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回收藏列表</response>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserFavorites(string userId)
    {
        _logger.LogInformation("Getting user favorites for user: {UserId}", userId);
        var favorites = await _favoriteService.GetUserFavoritesAsync(Guid.Parse(userId));
        return Ok(ApiResponse.Ok(favorites, "User favorites retrieved successfully"));
    }

    /// <summary>
    /// 获取用户是否收藏某句子
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <returns>是否收藏</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserFavorites/user/guid/sentence/guid/is-favorited
    ///     {
    ///         "success": true,
    ///         "data": true,
    ///         "message": "Favorite status checked successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">检查完成，返回是否收藏</response>
    [HttpGet("user/{userId}/sentence/{sentenceId}/is-favorited")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> IsFavorited(string userId, string sentenceId)
    {
        _logger.LogInformation("Checking if user {UserId} favorited sentence {SentenceId}", userId, sentenceId);
        var isFavorited = await _favoriteService.IsFavoritedAsync(Guid.Parse(userId), Guid.Parse(sentenceId));
        return Ok(ApiResponse.Ok(isFavorited, "Favorite status checked successfully"));
    }

    /// <summary>
    /// 添加收藏
    /// </summary>
    /// <param name="request">收藏请求</param>
    /// <returns>收藏结果</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/UserFavorites
    ///     {
    ///         "userId": "guid",
    ///         "sentenceId": "guid",
    ///         "note": "备注"
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
    ///             "sentenceId": "guid",
    ///             "note": "备注"
    ///         },
    ///         "message": "Favorite added successfully",
    ///         "statusCode": 201
    ///     }
    /// </remarks>
    /// <response code="201">添加成功，返回收藏详情</response>
    /// <response code="400">添加失败，输入无效</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequestDto request)
    {
        _logger.LogInformation("Adding favorite for user {UserId} and sentence {SentenceId}", request.UserId, request.SentenceId);
        var favorite = await _favoriteService.AddFavoriteAsync(request);
        return CreatedAtAction(nameof(GetFavorite), new { userId = request.UserId, sentenceId = request.SentenceId }, ApiResponse.Ok(favorite, "Favorite added successfully", StatusCodes.Status201Created));
    }

    /// <summary>
    /// 删除收藏
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <returns>删除结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     DELETE /api/UserFavorites/user/guid/sentence/guid
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "Favorite removed successfully",
    ///         "statusCode": 204
    ///     }
    /// </remarks>
    /// <response code="204">删除成功</response>
    /// <response code="404">收藏不存在</response>
    [HttpDelete("user/{userId}/sentence/{sentenceId}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFavorite(string userId, string sentenceId)
    {
        _logger.LogInformation("Removing favorite for user {UserId} and sentence {SentenceId}", userId, sentenceId);
        var result = await _favoriteService.RemoveFavoriteAsync(Guid.Parse(userId), Guid.Parse(sentenceId));
        if (!result)
        {
            return Ok(ApiResponse.Error("Favorite not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.NoContent("Favorite removed successfully"));
    }

    /// <summary>
    /// 获取收藏详情
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <returns>收藏详情</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserFavorites/user/guid/sentence/guid
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "userId": "guid",
    ///             "sentenceId": "guid",
    ///             "note": "备注"
    ///         },
    ///         "message": "Favorite retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回收藏详情</response>
    /// <response code="404">收藏不存在</response>
    [HttpGet("user/{userId}/sentence/{sentenceId}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFavorite(string userId, string sentenceId)
    {
        _logger.LogInformation("Getting favorite for user {UserId} and sentence {SentenceId}", userId, sentenceId);
        var favorite = await _favoriteService.GetFavoriteAsync(Guid.Parse(userId), Guid.Parse(sentenceId));
        if (favorite == null)
        {
            return Ok(ApiResponse.Error("Favorite not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(favorite, "Favorite retrieved successfully"));
    }

    /// <summary>
    /// 获取热门收藏句子
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <returns>热门收藏列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserFavorites/popular?limit=10
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "userId": "guid",
    ///                 "sentenceId": "guid",
    ///                 "note": "备注"
    ///             }
    ///         ],
    ///         "message": "Popular favorites retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回热门收藏列表</response>
    [HttpGet("popular")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPopularFavorites([FromQuery] int limit = 10)
    {
        _logger.LogInformation("Getting popular favorites with limit: {Limit}", limit);
        var favorites = await _favoriteService.GetPopularFavoritesAsync(limit);
        return Ok(ApiResponse.Ok(favorites, "Popular favorites retrieved successfully"));
    }

    /// <summary>
    /// 更新收藏备注
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="note">备注</param>
    /// <returns>更新结果</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     PUT /api/UserFavorites/user/guid/sentence/guid/note
    ///     {
    ///         "note": "更新后的备注"
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "Favorite note updated successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">更新成功</response>
    /// <response code="404">收藏不存在</response>
    [HttpPut("user/{userId}/sentence/{sentenceId}/note")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFavoriteNote(string userId, string sentenceId, [FromBody] string note)
    {
        _logger.LogInformation("Updating favorite note for user {UserId} and sentence {SentenceId}", userId, sentenceId);
        var result = await _favoriteService.UpdateFavoriteNoteAsync(Guid.Parse(userId), Guid.Parse(sentenceId), note);
        if (!result)
        {
            return Ok(ApiResponse.Error("Favorite not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(null, "Favorite note updated successfully"));
    }

    /// <summary>
    /// 获取用户收藏统计
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>收藏统计</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/UserFavorites/user/guid/stats
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "totalFavorites": 10,
    ///             "latestFavoriteDate": "2024-01-01"
    ///         },
    ///         "message": "User favorite stats retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回收藏统计</response>
    [HttpGet("user/{userId}/stats")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserFavoriteStats(string userId)
    {
        _logger.LogInformation("Getting user favorite stats for user: {UserId}", userId);
        var stats = await _favoriteService.GetUserFavoriteStatsAsync(Guid.Parse(userId));
        return Ok(ApiResponse.Ok(stats, "User favorite stats retrieved successfully"));
    }
}
