namespace Chet.CCLR.WebApi.DTOs;

/// <summary>
/// 爬虫数据源配置
/// </summary>
public class CrawlSourceConfig
{
    /// <summary>
    /// 数据源ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 数据源名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 数据源类型
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 基础URL
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// 目录页URL
    /// </summary>
    public string CatalogUrl { get; set; } = string.Empty;

    /// <summary>
    /// 详情页URL模板
    /// </summary>
    public string DetailUrlPattern { get; set; } = string.Empty;

    /// <summary>
    /// HTTP请求头
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();

    /// <summary>
    /// 请求延迟（毫秒）
    /// </summary>
    public int DelayMilliseconds { get; set; } = 1000;

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}
