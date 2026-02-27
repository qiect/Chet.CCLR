using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Classic;
using Chet.CCLR.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 经典章节控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("提供经典章节管理相关的API接口，包括获取、创建、更新和删除章节")]
public class ClassicChaptersController : ControllerBase
{
    /// <summary>
    /// 章节服务，用于处理章节相关的业务逻辑
    /// </summary>
    private readonly IClassicChapterService _chapterService;

    /// <summary>
    /// 日志记录器，用于记录控制器操作日志
    /// </summary>
    private readonly ILogger<ClassicChaptersController> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="chapterService">章节服务</param>
    public ClassicChaptersController(IClassicChapterService chapterService, ILogger<ClassicChaptersController> logger)
    {
        _chapterService = chapterService;
        _logger = logger;
    }

    /// <summary>
    /// 根据书籍ID获取章节列表
    /// </summary>
    /// <param name="bookId">书籍ID</param>
    /// <returns>章节列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicChapters/book/guid
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "bookId": "guid",
    ///                 "title": "第一回",
    ///                 "order": 1
    ///             },
    ///             {
    ///                 "id": "guid",
    ///                 "bookId": "guid",
    ///                 "title": "第二回",
    ///                 "order": 2
    ///             }
    ///         ],
    ///         "message": "Chapters retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回章节列表</response>
    [HttpGet("book/{bookId}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChaptersByBookId(string bookId)
    {
        _logger.LogInformation("Getting chapters for book with id: {BookId}", bookId);
        var chapters = await _chapterService.GetChaptersByBookIdAsync(Guid.Parse(bookId));
        return Ok(ApiResponse.Ok(chapters, "Chapters retrieved successfully"));
    }

    /// <summary>
    /// 根据章节ID获取章节详情
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <returns>章节详情</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicChapters/guid
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "bookId": "guid",
    ///             "title": "第一回",
    ///             "order": 1
    ///         },
    ///         "message": "Chapter retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回章节详情</response>
    /// <response code="404">章节不存在</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetChapter(string id)
    {
        _logger.LogInformation("Getting chapter with id: {Id}", id);
        var chapter = await _chapterService.GetChapterByIdAsync(Guid.Parse(id));
        if (chapter == null)
        {
            return Ok(ApiResponse.Error("Chapter not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(chapter, "Chapter retrieved successfully"));
    }

    /// <summary>
    /// 获取带句子的章节详情
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <returns>章节详情包含句子</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicChapters/guid/with-sentences
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "bookId": "guid",
    ///             "title": "第一回",
    ///             "order": 1,
    ///             "sentences": [
    ///                 {
    ///                     "id": "guid",
    ///                     "content": "句子内容",
    ///                     "translation": "翻译"
    ///                 }
    ///             ]
    ///         },
    ///         "message": "Chapter with sentences retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回章节详情包含句子</response>
    /// <response code="404">章节不存在</response>
    [HttpGet("{id}/with-sentences")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetChapterWithSentences(string id)
    {
        _logger.LogInformation("Getting chapter with sentences for id: {Id}", id);
        var chapter = await _chapterService.GetChapterWithSentencesAsync(Guid.Parse(id));
        if (chapter == null)
        {
            return Ok(ApiResponse.Error("Chapter not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(chapter, "Chapter with sentences retrieved successfully"));
    }

    /// <summary>
    /// 创建新章节
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的章节</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/ClassicChapters
    ///     {
    ///         "bookId": "guid",
    ///         "title": "新章节",
    ///         "order": 3
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 201 Created
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "bookId": "guid",
    ///             "title": "新章节",
    ///             "order": 3
    ///         },
    ///         "message": "Chapter created successfully",
    ///         "statusCode": 201
    ///     }
    /// </remarks>
    /// <response code="201">创建成功，返回创建的章节</response>
    /// <response code="400">创建失败，输入无效</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateChapter([FromBody] CreateChapterRequestDto request)
    {
        _logger.LogInformation("Creating new chapter: {Title}", request.Title);
        var chapter = await _chapterService.CreateChapterAsync(request);
        return CreatedAtAction(nameof(GetChapter), new { id = chapter.Id }, ApiResponse.Ok(chapter, "Chapter created successfully", StatusCodes.Status201Created));
    }

    /// <summary>
    /// 更新章节信息
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新的章节</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     PUT /api/ClassicChapters/guid
    ///     {
    ///         "title": "更新后的章节名",
    ///         "order": 2
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "bookId": "guid",
    ///             "title": "更新后的章节名",
    ///             "order": 2
    ///         },
    ///         "message": "Chapter updated successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">更新成功，返回更新的章节</response>
    /// <response code="404">章节不存在</response>
    /// <response code="400">更新失败，输入无效</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateChapter(string id, [FromBody] UpdateChapterRequestDto request)
    {
        _logger.LogInformation("Updating chapter with id: {Id}", id);
        var chapter = await _chapterService.UpdateChapterAsync(Guid.Parse(id), request);
        if (chapter == null)
        {
            return Ok(ApiResponse.Error("Chapter not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(chapter, "Chapter updated successfully"));
    }

    /// <summary>
    /// 删除章节
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <returns>删除结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     DELETE /api/ClassicChapters/guid
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "Chapter deleted successfully",
    ///         "statusCode": 204
    ///     }
    /// </remarks>
    /// <response code="204">删除成功</response>
    /// <response code="404">章节不存在</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteChapter(string id)
    {
        _logger.LogInformation("Deleting chapter with id: {Id}", id);
        var result = await _chapterService.DeleteChapterAsync(Guid.Parse(id));
        if (!result)
        {
            return Ok(ApiResponse.Error("Chapter not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.NoContent("Chapter deleted successfully"));
    }

    /// <summary>
    /// 获取章节的句子数量
    /// </summary>
    /// <param name="id">章节ID</param>
    /// <returns>句子数量</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicChapters/guid/sentence-count
    ///     {
    ///         "success": true,
    ///         "data": 10,
    ///         "message": "Sentence count retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回句子数量</response>
    [HttpGet("{id}/sentence-count")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSentenceCountByChapterId(string id)
    {
        _logger.LogInformation("Getting sentence count for chapter with id: {Id}", id);
        var count = await _chapterService.GetSentenceCountByChapterIdAsync(Guid.Parse(id));
        return Ok(ApiResponse.Ok(count, "Sentence count retrieved successfully"));
    }
}
