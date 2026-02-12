using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Request.Listen;
using Chet.CCLR.WebApi.DTOs.Response.Listen;
using Chet.CCLR.WebApi.DTOs.Response.Classic;
using Microsoft.AspNetCore.Mvc;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 用户听读进度控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserListenProgressController : ControllerBase
{
    private readonly IUserListenProgressService _progressService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="progressService">进度服务</param>
    public UserListenProgressController(IUserListenProgressService progressService)
    {
        _progressService = progressService;
    }

    /// <summary>
    /// 获取用户的书籍进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <returns>进度信息</returns>
    [HttpGet("user/{userId}/book/{bookId}")]
    public async Task<ActionResult<ProgressResponseDto>> GetUserProgress(string userId, string bookId)
    {
        var progress = await _progressService.GetUserProgressAsync(Guid.Parse(userId), Guid.Parse(bookId));
        if (progress == null)
        {
            return NotFound();
        }
        return Ok(progress);
    }

    /// <summary>
    /// 获取用户所有书籍进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>进度列表</returns>
    [HttpGet("user/{userId}/all")]
    public async Task<ActionResult<IEnumerable<ProgressResponseDto>>> GetUserAllProgress(string userId)
    {
        var progresses = await _progressService.GetUserAllProgressAsync(Guid.Parse(userId));
        return Ok(progresses);
    }

    /// <summary>
    /// 更新用户进度
    /// </summary>
    /// <param name="request">更新请求</param>
    /// <returns>更新后的进度</returns>
    [HttpPost]
    public async Task<ActionResult<ProgressResponseDto>> UpdateUserProgress([FromBody] UpdateProgressRequestDto request)
    {
        var progress = await _progressService.UpdateUserProgressAsync(request);
        return Ok(progress);
    }

    /// <summary>
    /// 重置用户书籍进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <returns>重置结果</returns>
    [HttpPost("reset/user/{userId}/book/{bookId}")]
    public async Task<IActionResult> ResetUserProgress(string userId, string bookId)
    {
        var result = await _progressService.ResetUserProgressAsync(Guid.Parse(userId), Guid.Parse(bookId));
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// 获取用户当前学习的句子
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <returns>当前句子信息</returns>
    [HttpGet("user/{userId}/book/{bookId}/current-sentence")]
    public async Task<ActionResult<SentenceResponseDto>> GetCurrentSentence(string userId, string bookId)
    {
        var sentence = await _progressService.GetCurrentSentenceAsync(Guid.Parse(userId), Guid.Parse(bookId));
        if (sentence == null)
        {
            return NotFound();
        }
        return Ok(sentence);
    }

    /// <summary>
    /// 获取用户学习统计
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>学习统计数据</returns>
    [HttpGet("user/{userId}/stats")]
    public async Task<ActionResult<LearningStatsResponseDto>> GetUserLearningStats(string userId)
    {
        var stats = await _progressService.GetUserLearningStatsAsync(Guid.Parse(userId));
        return Ok(stats);
    }

    /// <summary>
    /// 删除用户进度
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="bookId">书籍ID</param>
    /// <returns>删除结果</returns>
    [HttpDelete("user/{userId}/book/{bookId}")]
    public async Task<IActionResult> DeleteUserProgress(string userId, string bookId)
    {
        var result = await _progressService.DeleteUserProgressAsync(Guid.Parse(userId), Guid.Parse(bookId));
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}