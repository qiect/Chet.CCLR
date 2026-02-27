using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Classic;
using Chet.CCLR.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 经典句子控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("提供经典句子管理相关的API接口，包括获取、创建、更新和删除句子")]
public class ClassicSentencesController : ControllerBase
{
    /// <summary>
    /// 句子服务，用于处理句子相关的业务逻辑
    /// </summary>
    private readonly IClassicSentenceService _sentenceService;

    /// <summary>
    /// 日志记录器，用于记录控制器操作日志
    /// </summary>
    private readonly ILogger<ClassicSentencesController> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sentenceService">句子服务</param>
    public ClassicSentencesController(IClassicSentenceService sentenceService, ILogger<ClassicSentencesController> logger)
    {
        _sentenceService = sentenceService;
        _logger = logger;
    }

    /// <summary>
    /// 根据章节ID获取句子列表
    /// </summary>
    /// <param name="chapterId">章节ID</param>
    /// <returns>句子列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicSentences/chapter/guid
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "chapterId": "guid",
    ///                 "content": "句子内容",
    ///                 "translation": "翻译"
    ///             },
    ///             {
    ///                 "id": "guid",
    ///                 "chapterId": "guid",
    ///                 "content": "句子内容2",
    ///                 "translation": "翻译2"
    ///             }
    ///         ],
    ///         "message": "Sentences retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回句子列表</response>
    [HttpGet("chapter/{chapterId}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSentencesByChapterId(string chapterId)
    {
        _logger.LogInformation("Getting sentences for chapter with id: {ChapterId}", chapterId);
        var sentences = await _sentenceService.GetSentencesByChapterIdAsync(Guid.Parse(chapterId));
        return Ok(ApiResponse.Ok(sentences, "Sentences retrieved successfully"));
    }

    /// <summary>
    /// 根据句子ID获取句子详情
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <returns>句子详情</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicSentences/guid
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "chapterId": "guid",
    ///             "content": "句子内容",
    ///             "translation": "翻译"
    ///         },
    ///         "message": "Sentence retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回句子详情</response>
    /// <response code="404">句子不存在</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSentence(string id)
    {
        _logger.LogInformation("Getting sentence with id: {Id}", id);
        var sentence = await _sentenceService.GetSentenceByIdAsync(Guid.Parse(id));
        if (sentence == null)
        {
            return Ok(ApiResponse.Error("Sentence not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(sentence, "Sentence retrieved successfully"));
    }

    /// <summary>
    /// 获取随机句子
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <returns>随机句子列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicSentences/random?limit=5
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "chapterId": "guid",
    ///                 "content": "随机句子1",
    ///                 "translation": "随机翻译1"
    ///             }
    ///         ],
    ///         "message": "Random sentences retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回随机句子列表</response>
    [HttpGet("random")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRandomSentences([FromQuery] int limit = 5)
    {
        _logger.LogInformation("Getting random sentences with limit: {Limit}", limit);
        var sentences = await _sentenceService.GetRandomSentencesAsync(limit);
        return Ok(ApiResponse.Ok(sentences, "Random sentences retrieved successfully"));
    }

    /// <summary>
    /// 创建新句子
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的句子</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/ClassicSentences
    ///     {
    ///         "chapterId": "guid",
    ///         "content": "新句子",
    ///         "translation": "新翻译"
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 201 Created
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "chapterId": "guid",
    ///             "content": "新句子",
    ///             "translation": "新翻译"
    ///         },
    ///         "message": "Sentence created successfully",
    ///         "statusCode": 201
    ///     }
    /// </remarks>
    /// <response code="201">创建成功，返回创建的句子</response>
    /// <response code="400">创建失败，输入无效</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSentence([FromBody] CreateSentenceRequestDto request)
    {
        _logger.LogInformation("Creating new sentence: {Content}", request.Content);
        var sentence = await _sentenceService.CreateSentenceAsync(request);
        return CreatedAtAction(nameof(GetSentence), new { id = sentence.Id }, ApiResponse.Ok(sentence, "Sentence created successfully", StatusCodes.Status201Created));
    }

    /// <summary>
    /// 更新句子信息
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新的句子</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     PUT /api/ClassicSentences/guid
    ///     {
    ///         "content": "更新后的句子",
    ///         "translation": "更新后的翻译"
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "chapterId": "guid",
    ///             "content": "更新后的句子",
    ///             "translation": "更新后的翻译"
    ///         },
    ///         "message": "Sentence updated successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">更新成功，返回更新的句子</response>
    /// <response code="404">句子不存在</response>
    /// <response code="400">更新失败，输入无效</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSentence(string id, [FromBody] UpdateSentenceRequestDto request)
    {
        _logger.LogInformation("Updating sentence with id: {Id}", id);
        var sentence = await _sentenceService.UpdateSentenceAsync(Guid.Parse(id), request);
        if (sentence == null)
        {
            return Ok(ApiResponse.Error("Sentence not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(sentence, "Sentence updated successfully"));
    }

    /// <summary>
    /// 删除句子
    /// </summary>
    /// <param name="id">句子ID</param>
    /// <returns>删除结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     DELETE /api/ClassicSentences/guid
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "Sentence deleted successfully",
    ///         "statusCode": 204
    ///     }
    /// </remarks>
    /// <response code="204">删除成功</response>
    /// <response code="404">句子不存在</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSentence(string id)
    {
        _logger.LogInformation("Deleting sentence with id: {Id}", id);
        var result = await _sentenceService.DeleteSentenceAsync(Guid.Parse(id));
        if (!result)
        {
            return Ok(ApiResponse.Error("Sentence not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.NoContent("Sentence deleted successfully"));
    }

    /// <summary>
    /// 搜索句子内容
    /// </summary>
    /// <param name="keyword">关键词</param>
    /// <returns>搜索结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicSentences/search?keyword=关键词
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "chapterId": "guid",
    ///                 "content": "包含关键词的句子",
    ///                 "translation": "翻译"
    ///             }
    ///         ],
    ///         "message": "Sentences searched successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">搜索成功，返回搜索结果</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchSentences([FromQuery] string keyword = "")
    {
        _logger.LogInformation("Searching sentences with keyword: {Keyword}", keyword);
        var sentences = await _sentenceService.SearchSentencesAsync(keyword);
        return Ok(ApiResponse.Ok(sentences, "Sentences searched successfully"));
    }

    /// <summary>
    /// 获取句子总数
    /// </summary>
    /// <returns>句子总数</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicSentences/total-count
    ///     {
    ///         "success": true,
    ///         "data": 1000,
    ///         "message": "Total sentence count retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回句子总数</response>
    [HttpGet("total-count")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalSentenceCount()
    {
        _logger.LogInformation("Getting total sentence count");
        var count = await _sentenceService.GetTotalSentenceCountAsync();
        return Ok(ApiResponse.Ok(count, "Total sentence count retrieved successfully"));
    }
}
