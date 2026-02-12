using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.DTOs.Request.Classic;
using Chet.CCLR.WebApi.DTOs.Response.Classic;
using Microsoft.AspNetCore.Mvc;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 经典书籍控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClassicBooksController : ControllerBase
{
    private readonly IClassicBookService _bookService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bookService">书籍服务</param>
    public ClassicBooksController(IClassicBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// 获取所有经典书籍
    /// </summary>
    /// <returns>书籍列表</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    /// <summary>
    /// 根据ID获取书籍详情
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <returns>书籍详情</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponseDto>> GetBook(string id)
    {
        var book = await _bookService.GetBookByIdAsync(Guid.Parse(id));
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    /// <summary>
    /// 根据分类获取书籍
    /// </summary>
    /// <param name="category">分类</param>
    /// <returns>书籍列表</returns>
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetBooksByCategory(string category)
    {
        var books = await _bookService.GetBooksByCategoryAsync(category);
        return Ok(books);
    }

    /// <summary>
    /// 获取推荐书籍
    /// </summary>
    /// <param name="limit">限制数量</param>
    /// <returns>推荐书籍列表</returns>
    [HttpGet("recommended")]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetRecommendedBooks([FromQuery] int limit = 10)
    {
        var books = await _bookService.GetRecommendedBooksAsync(limit);
        return Ok(books);
    }

    /// <summary>
    /// 搜索书籍
    /// </summary>
    /// <param name="keyword">关键词</param>
    /// <returns>搜索结果</returns>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> SearchBooks([FromQuery] string keyword = "")
    {
        var books = await _bookService.SearchBooksAsync(keyword);
        return Ok(books);
    }

    /// <summary>
    /// 创建新书籍
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的书籍</returns>
    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> CreateBook([FromBody] CreateBookRequestDto request)
    {
        var book = await _bookService.CreateBookAsync(request);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    /// <summary>
    /// 更新书籍信息
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新的书籍</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<BookResponseDto>> UpdateBook(string id, [FromBody] UpdateBookRequestDto request)
    {
        var book = await _bookService.UpdateBookAsync(Guid.Parse(id), request);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    /// <summary>
    /// 删除书籍
    /// </summary>
    /// <param name="id">书籍ID</param>
    /// <returns>删除结果</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(string id)
    {
        var result = await _bookService.DeleteBookAsync(Guid.Parse(id));
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// 获取分页书籍列表
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="size">每页大小</param>
    /// <returns>分页结果</returns>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<BookResponseDto>>> GetPagedBooks([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var pagedResult = await _bookService.GetPagedBooksAsync(page, size);
        return Ok(pagedResult);
    }
}