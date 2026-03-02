using Chet.CCLR.WebApi.DTOs.Craw;

namespace Chet.CCLR.WebApi.Contracts.IServices;

/// <summary>
/// 爬虫数据源接口
/// </summary>
public interface ICrawlSource
{
    /// <summary>
    /// 数据源ID
    /// </summary>
    string SourceId { get; }

    /// <summary>
    /// 爬取目录
    /// </summary>
    /// <param name="catalogUrl">目录页URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>爬取结果</returns>
    Task<CrawlResult> CrawlCatalogAsync(string catalogUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// 爬取章节
    /// </summary>
    /// <param name="chapterUrl">章节URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>爬取结果</returns>
    Task<CrawlResult> CrawlChapterAsync(string catalogUrl, string chapterUrl, CancellationToken cancellationToken = default);
}
