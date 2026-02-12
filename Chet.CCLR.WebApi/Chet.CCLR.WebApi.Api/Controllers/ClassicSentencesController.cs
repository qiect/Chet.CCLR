using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Request.Classic;
using Chet.CCLR.WebApi.DTOs.Response.Classic;
using Microsoft.AspNetCore.Mvc;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 经典句子控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClassicSentencesController : ControllerBase
{
    private readonly IClassicSentenceService _sentenceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sentenceService">句子服务</param>
    public ClassicSentencesController(IClassicSentenceService sentenceService)
    {
        _sentenceService = sentenceService;
    }

    /// <summary>
    /// 根据章节ID获取句子列表
    /// </summary>
    /// <param name="chapterId">章节ID</param>
    /// <returns>句子列表</returns>
    [HttpGet("chapter/{chapterId}")]
    public async Task<ActionResult<IEnumerable<SentenceResponseDto>>> GetSentencesByChapterId(string chapterId)
    {
        var sentences = await _sentenceService.GetSentencesByChapterIdAsync(Guid.Parse(chapterId));
        return Ok(sentences);
    }

    /// <summary>
    /// 根据句子ID获取句子详情
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <returns>句子详情</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SentenceResponseDto>> GetSentence(string id)
    {
        var sentence = await _sentenceService.GetSentenceByIdAsync(Guid.Parse(id));
        if (sentence == null)
        {
            return NotFound();
        }
        return Ok(sentence);
    }

    /// <summary>
    /// 获取随机句子
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <returns>随机句子列表</returns>
    [HttpGet("random")]
    public async Task<ActionResult<IEnumerable<SentenceResponseDto>>> GetRandomSentences([FromQuery] int limit = 5)
    {
        var sentences = await _sentenceService.GetRandomSentencesAsync(limit);
        return Ok(sentences);
    }

    /// <summary>
    /// 创建新句子
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的句子</returns>
    [HttpPost]
    public async Task<ActionResult<SentenceResponseDto>> CreateSentence([FromBody] CreateSentenceRequestDto request)
    {
        var sentence = await _sentenceService.CreateSentenceAsync(request);
        return CreatedAtAction(nameof(GetSentence), new { id = sentence.Id }, sentence);
    }

    /// <summary>
    /// 更新句子信息
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新的句子</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<SentenceResponseDto>> UpdateSentence(string id, [FromBody] UpdateSentenceRequestDto request)
    {
        var sentence = await _sentenceService.UpdateSentenceAsync(Guid.Parse(id), request);
        if (sentence == null)
        {
            return NotFound();
        }
        return Ok(sentence);
    }

    /// <summary>
    /// 删除句子
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <returns>删除结果</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSentence(string id)
    {
        var result = await _sentenceService.DeleteSentenceAsync(Guid.Parse(id));
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// 搜索句子内容
    /// </summary>
    /// <param name="keyword">关键词</param>
    /// <returns>搜索结果</returns>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<SentenceResponseDto>>> SearchSentences([FromQuery] string keyword = "")
    {
        var sentences = await _sentenceService.SearchSentencesAsync(keyword);
        return Ok(sentences);
    }

    /// <summary>
    /// 获取句子总数
    /// </summary>
    /// <returns>句子总数</returns>
    [HttpGet("total-count")]
    public async Task<ActionResult<int>> GetTotalSentenceCount()
    {
        var count = await _sentenceService.GetTotalSentenceCountAsync();
        return Ok(count);
    }
}