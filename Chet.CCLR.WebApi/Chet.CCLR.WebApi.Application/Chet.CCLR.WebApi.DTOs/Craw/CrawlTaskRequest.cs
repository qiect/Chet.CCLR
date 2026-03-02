namespace Chet.CCLR.WebApi.DTOs.Craw;

/// <summary>
/// 爬取任务请求
/// </summary>
public class CrawlTaskRequest
{
    /// <summary>
    /// 数据源 ID
    /// </summary>
    public required string SourceId { get; set; }

    /// <summary>
    /// 目录页 URL（可选，默认使用数据源配置的 CatalogUrl）
    /// </summary>
    public string? CatalogUrl { get; set; }

    /// <summary>
    /// 是否覆盖已有数据
    /// </summary>
    public bool Overwrite { get; set; } = false;

    /// <summary>
    /// 音频文件目录（相对路径）
    /// </summary>
    public string? AudioDirectory { get; set; }
}
