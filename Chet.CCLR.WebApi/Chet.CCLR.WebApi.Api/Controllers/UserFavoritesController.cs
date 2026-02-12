using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Request.Listen;
using Chet.CCLR.WebApi.DTOs.Response.Listen;
using Microsoft.AspNetCore.Mvc;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 用户收藏控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserFavoritesController : ControllerBase
{
    private readonly IUserFavoriteSentenceService _favoriteService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="favoriteService">收藏服务</param>
    public UserFavoritesController(IUserFavoriteSentenceService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    /// <summary>
    /// 获取用户的收藏列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>收藏列表</returns>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<FavoriteResponseDto>>> GetUserFavorites(string userId)
    {
        var favorites = await _favoriteService.GetUserFavoritesAsync(Guid.Parse(userId));
        return Ok(favorites);
    }

    /// <summary>
    /// 获取用户是否收藏某句子
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <returns>是否收藏</returns>
    [HttpGet("user/{userId}/sentence/{sentenceId}/is-favorited")]
    public async Task<ActionResult<bool>> IsFavorited(string userId, string sentenceId)
    {
        var isFavorited = await _favoriteService.IsFavoritedAsync(Guid.Parse(userId), Guid.Parse(sentenceId));
        return Ok(isFavorited);
    }

    /// <summary>
    /// 添加收藏
    /// </summary>
    /// <param name="request">收藏请求</param>
    /// <returns>收藏结果</returns>
    [HttpPost]
    public async Task<ActionResult<FavoriteResponseDto>> AddFavorite([FromBody] AddFavoriteRequestDto request)
    {
        var favorite = await _favoriteService.AddFavoriteAsync(request);
        return CreatedAtAction(nameof(GetFavorite), new { userId = request.UserId, sentenceId = request.SentenceId }, favorite);
    }

    /// <summary>
    /// 删除收藏
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <returns>删除结果</returns>
    [HttpDelete("user/{userId}/sentence/{sentenceId}")]
    public async Task<IActionResult> RemoveFavorite(string userId, string sentenceId)
    {
        var result = await _favoriteService.RemoveFavoriteAsync(Guid.Parse(userId), Guid.Parse(sentenceId));
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// 获取收藏详情
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <returns>收藏详情</returns>
    [HttpGet("user/{userId}/sentence/{sentenceId}")]
    public async Task<ActionResult<FavoriteResponseDto>> GetFavorite(string userId, string sentenceId)
    {
        var favorite = await _favoriteService.GetFavoriteAsync(Guid.Parse(userId), Guid.Parse(sentenceId));
        if (favorite == null)
        {
            return NotFound();
        }
        return Ok(favorite);
    }

    /// <summary>
    /// 获取热门收藏句子
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <returns>热门收藏列表</returns>
    [HttpGet("popular")]
    public async Task<ActionResult<IEnumerable<FavoriteResponseDto>>> GetPopularFavorites([FromQuery] int limit = 10)
    {
        var favorites = await _favoriteService.GetPopularFavoritesAsync(limit);
        return Ok(favorites);
    }

    /// <summary>
    /// 更新收藏备注
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="sentenceId">句子ID</param>
    /// <param name="note">备注</param>
    /// <returns>更新结果</returns>
    [HttpPut("user/{userId}/sentence/{sentenceId}/note")]
    public async Task<IActionResult> UpdateFavoriteNote(string userId, string sentenceId, [FromBody] string note)
    {
        var result = await _favoriteService.UpdateFavoriteNoteAsync(Guid.Parse(userId), Guid.Parse(sentenceId), note);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// 获取用户收藏统计
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>收藏统计</returns>
    [HttpGet("user/{userId}/stats")]
    public async Task<ActionResult<FavoriteStatsResponseDto>> GetUserFavoriteStats(string userId)
    {
        var stats = await _favoriteService.GetUserFavoriteStatsAsync(Guid.Parse(userId));
        return Ok(stats);
    }
}