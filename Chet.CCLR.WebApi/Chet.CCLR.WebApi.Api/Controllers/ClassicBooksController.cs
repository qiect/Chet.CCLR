using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.DTOs.Classic;
using Chet.CCLR.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 经典书籍控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("提供经典书籍管理相关的API接口，包括获取、创建、更新和删除书籍")]
public class ClassicBooksController : ControllerBase
{
    /// <summary>
    /// 书籍服务，用于处理书籍相关的业务逻辑
    /// </summary>
    private readonly IClassicBookService _bookService;

    /// <summary>
    /// 日志记录器，用于记录控制器操作日志
    /// </summary>
    private readonly ILogger<ClassicBooksController> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bookService">书籍服务</param>
    public ClassicBooksController(IClassicBookService bookService, ILogger<ClassicBooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    /// <summary>
    /// 获取所有经典书籍
    /// </summary>
    /// <returns>书籍列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicBooks
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "title": "红楼梦",
    ///                 "author": "曹雪芹",
    ///                 "category": "古典文学"
    ///             },
    ///             {
    ///                 "id": "guid",
    ///                 "title": "西游记",
    ///                 "author": "吴承恩",
    ///                 "category": "古典文学"
    ///             }
    ///         ],
    ///         "message": "Books retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回书籍列表</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBooks()
    {
        _logger.LogInformation("Getting all classic books");
        var books = await _bookService.GetAllBooksAsync();
        return Ok(ApiResponse.Ok(books, "Books retrieved successfully"));
    }

    /// <summary>
    /// 根据ID获取书籍详情
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <returns>书籍详情</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicBooks/guid
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "title": "红楼梦",
    ///             "author": "曹雪芹",
    ///             "category": "古典文学"
    ///         },
    ///         "message": "Book retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回书籍详情</response>
    /// <response code="404">书籍不存在</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBook(string id)
    {
        _logger.LogInformation("Getting book with id: {Id}", id);
        var book = await _bookService.GetBookByIdAsync(Guid.Parse(id));
        if (book == null)
        {
            return Ok(ApiResponse.Error("Book not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(book, "Book retrieved successfully"));
    }

    /// <summary>
    /// 根据分类获取书籍
    /// </summary>
    /// <param name="category">分类</param>
    /// <returns>书籍列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicBooks/category/古典文学
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "title": "红楼梦",
    ///                 "author": "曹雪芹",
    ///                 "category": "古典文学"
    ///             }
    ///         ],
    ///         "message": "Books retrieved by category",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回书籍列表</response>
    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBooksByCategory(string category)
    {
        _logger.LogInformation("Getting books by category: {Category}", category);
        var books = await _bookService.GetBooksByCategoryAsync(category);
        return Ok(ApiResponse.Ok(books, "Books retrieved by category"));
    }

    /// <summary>
    /// 获取推荐书籍
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <returns>推荐书籍列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicBooks/recommended?limit=5
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "title": "红楼梦",
    ///                 "author": "曹雪芹",
    ///                 "category": "古典文学"
    ///             }
    ///         ],
    ///         "message": "Recommended books retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回推荐书籍列表</response>
    [HttpGet("recommended")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecommendedBooks([FromQuery] int limit = 10)
    {
        _logger.LogInformation("Getting recommended books with limit: {Limit}", limit);
        var books = await _bookService.GetRecommendedBooksAsync(limit);
        return Ok(ApiResponse.Ok(books, "Recommended books retrieved successfully"));
    }

    /// <summary>
    /// 搜索书籍
    /// </summary>
    /// <param name="keyword">关键词</param>
    /// <returns>搜索结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicBooks/search?keyword=红楼梦
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "id": "guid",
    ///                 "title": "红楼梦",
    ///                 "author": "曹雪芹",
    ///                 "category": "古典文学"
    ///             }
    ///         ],
    ///         "message": "Books searched successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">搜索成功，返回搜索结果</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchBooks([FromQuery] string keyword = "")
    {
        _logger.LogInformation("Searching books with keyword: {Keyword}", keyword);
        var books = await _bookService.SearchBooksAsync(keyword);
        return Ok(ApiResponse.Ok(books, "Books searched successfully"));
    }

    /// <summary>
    /// 创建新书籍
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的书籍</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/ClassicBooks
    ///     {
    ///         "title": "新书",
    ///         "author": "作者",
    ///         "category": "分类"
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 201 Created
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "title": "新书",
    ///             "author": "作者",
    ///             "category": "分类"
    ///         },
    ///         "message": "Book created successfully",
    ///         "statusCode": 201
    ///     }
    /// </remarks>
    /// <response code="201">创建成功，返回创建的书籍</response>
    /// <response code="400">创建失败，输入无效</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookRequestDto request)
    {
        _logger.LogInformation("Creating new book: {Title}", request.Title);
        var book = await _bookService.CreateBookAsync(request);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, ApiResponse.Ok(book, "Book created successfully", StatusCodes.Status201Created));
    }

    /// <summary>
    /// 更新书籍信息
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新的书籍</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     PUT /api/ClassicBooks/guid
    ///     {
    ///         "title": "更新后的书名",
    ///         "author": "更新后的作者"
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "title": "更新后的书名",
    ///             "author": "更新后的作者",
    ///             "category": "古典文学"
    ///         },
    ///         "message": "Book updated successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">更新成功，返回更新的书籍</response>
    /// <response code="404">书籍不存在</response>
    /// <response code="400">更新失败，输入无效</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBook(string id, [FromBody] UpdateBookRequestDto request)
    {
        _logger.LogInformation("Updating book with id: {Id}", id);
        var book = await _bookService.UpdateBookAsync(Guid.Parse(id), request);
        if (book == null)
        {
            return Ok(ApiResponse.Error("Book not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(book, "Book updated successfully"));
    }

    /// <summary>
    /// 删除书籍
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <returns>删除结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     DELETE /api/ClassicBooks/guid
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "Book deleted successfully",
    ///         "statusCode": 204
    ///     }
    /// </remarks>
    /// <response code="204">删除成功</response>
    /// <response code="404">书籍不存在</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook(string id)
    {
        _logger.LogInformation("Deleting book with id: {Id}", id);
        var result = await _bookService.DeleteBookAsync(Guid.Parse(id));
        if (!result)
        {
            return Ok(ApiResponse.Error("Book not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.NoContent("Book deleted successfully"));
    }

    /// <summary>
    /// 获取分页书籍列表
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="size">每页大小</param>
    /// <returns>分页结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/ClassicBooks/paged?page=1&size=10
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "items": [...],
    ///             "total": 100,
    ///             "page": 1,
    ///             "size": 10,
    ///             "totalPages": 10
    ///         },
    ///         "message": "Paged books retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回分页结果</response>
    [HttpGet("paged")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagedBooks([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        _logger.LogInformation("Getting paged books: Page={Page}, Size={Size}", page, size);
        var pagedResult = await _bookService.GetPagedBooksAsync(page, size);
        return Ok(ApiResponse.Ok(pagedResult, "Paged books retrieved successfully"));
    }
}
