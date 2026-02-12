using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Request.Classic;
using Chet.CCLR.WebApi.DTOs.Response.Classic;
using Microsoft.AspNetCore.Mvc;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 经典章节控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClassicChaptersController : ControllerBase
{
    private readonly IClassicChapterService _chapterService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="chapterService">章节服务</param>
    public ClassicChaptersController(IClassicChapterService chapterService)
    {
        _chapterService = chapterService;
    }

    /// <summary>
    /// 根据书籍ID获取章节列表
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <returns>章节列表</returns>
    [HttpGet("book/{bookId}")]
    public async Task<ActionResult<IEnumerable<ChapterResponseDto>>> GetChaptersByBookId(string bookId)
    {
        var chapters = await _chapterService.GetChaptersByBookIdAsync(Guid.Parse(bookId));
        return Ok(chapters);
    }

    /// <summary>
    /// 根据章节ID获取章节详情
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <returns>章节详情</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ChapterResponseDto>> GetChapter(string id)
    {
        var chapter = await _chapterService.GetChapterByIdAsync(Guid.Parse(id));
        if (chapter == null)
        {
            return NotFound();
        }
        return Ok(chapter);
    }

    /// <summary>
    /// 获取带句子的章节详情
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <returns>章节详情包含句子</returns>
    [HttpGet("{id}/with-sentences")]
    public async Task<ActionResult<ChapterWithSentencesResponseDto>> GetChapterWithSentences(string id)
    {
        var chapter = await _chapterService.GetChapterWithSentencesAsync(Guid.Parse(id));
        if (chapter == null)
        {
            return NotFound();
        }
        return Ok(chapter);
    }

    /// <summary>
    /// 创建新章节
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的章节</returns>
    [HttpPost]
    public async Task<ActionResult<ChapterResponseDto>> CreateChapter([FromBody] CreateChapterRequestDto request)
    {
        var chapter = await _chapterService.CreateChapterAsync(request);
        return CreatedAtAction(nameof(GetChapter), new { id = chapter.Id }, chapter);
    }

    /// <summary>
    /// 更新章节信息
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新的章节</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ChapterResponseDto>> UpdateChapter(string id, [FromBody] UpdateChapterRequestDto request)
    {
        var chapter = await _chapterService.UpdateChapterAsync(Guid.Parse(id), request);
        if (chapter == null)
        {
            return NotFound();
        }
        return Ok(chapter);
    }

    /// <summary>
    /// 删除章节
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <returns>删除结果</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChapter(string id)
    {
        var result = await _chapterService.DeleteChapterAsync(Guid.Parse(id));
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// 获取章节的句子数量
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <returns>句子数量</returns>
    [HttpGet("{id}/sentence-count")]
    public async Task<ActionResult<int>> GetSentenceCountByChapterId(string id)
    {
        var count = await _chapterService.GetSentenceCountByChapterIdAsync(Guid.Parse(id));
        return Ok(count);
    }
}