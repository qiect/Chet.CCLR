using Chet.CCLR.WebApi.DTOs;

namespace Chet.CCLR.WebApi.Contracts.IServices;

/// <summary>
/// 爬虫服务接口
/// </summary>
public interface ICrawlService
{
    /// <summary>
    /// 获取所有可用的数据源配置
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>数据源配置列表</returns>
    Task<IEnumerable<CrawlSourceConfig>> GetAllSourcesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据ID获取数据源配置
    /// </summary>
    /// <param name="sourceId">数据源ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>数据源配置</returns>
    Task<CrawlSourceConfig?> GetSourceByIdAsync(string sourceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 执行爬取任务
    /// </summary>
    /// <param name="request">爬取任务请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>爬取结果</returns>
    Task<CrawlResult> CrawlAsync(CrawlTaskRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 导入爬取的数据到数据库
    /// </summary>
    /// <param name="result">爬取结果</param>
    /// <param name="overwrite">是否覆盖</param>
    /// <param name="audioDirectory">音频目录</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>导入结果</returns>
    Task<CrawlResult> ImportToDatabaseAsync(CrawlResult result, bool overwrite, string audioDirectory, CancellationToken cancellationToken = default);
}
