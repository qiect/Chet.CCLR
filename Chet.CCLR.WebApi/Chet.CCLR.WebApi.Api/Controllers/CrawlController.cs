using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Chet.CCLR.WebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CrawlController : ControllerBase
{
    private readonly ICrawlService _crawlService;

    public CrawlController(ICrawlService crawlService)
    {
        _crawlService = crawlService;
    }

    [HttpGet("sources")]
    public async Task<IActionResult> GetAllSources(CancellationToken cancellationToken = default)
    {
        var sources = await _crawlService.GetAllSourcesAsync(cancellationToken);
        return Ok(sources);
    }

    [HttpGet("sources/{sourceId}")]
    public async Task<IActionResult> GetSource(string sourceId, CancellationToken cancellationToken = default)
    {
        var source = await _crawlService.GetSourceByIdAsync(sourceId, cancellationToken);
        if (source == null)
        {
            return NotFound();
        }
        return Ok(source);
    }

    [HttpPost("crawl")]
    public async Task<IActionResult> Crawl([FromBody] CrawlTaskRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _crawlService.CrawlAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import([FromBody] CrawlResult result, bool overwrite = false, string audioDirectory = "道德经", CancellationToken cancellationToken = default)
    {
        var importResult = await _crawlService.ImportToDatabaseAsync(result, overwrite, audioDirectory, cancellationToken);
        return Ok(importResult);
    }
}
