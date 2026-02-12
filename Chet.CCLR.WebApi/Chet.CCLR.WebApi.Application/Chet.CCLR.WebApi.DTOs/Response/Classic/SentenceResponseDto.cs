namespace Chet.CCLR.WebApi.DTOs.Response.Classic;

/// <summary>
/// 句子响应DTO
/// </summary>
public class SentenceResponseDto
{
    /// <summary>
    /// 句子ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 拼音
    /// </summary>
    public string? Pinyin { get; set; }

    /// <summary>
    /// 注释
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// 翻译
    /// </summary>
    public string? Translation { get; set; }

    /// <summary>
    /// 音频URL
    /// </summary>
    public string AudioUrl { get; set; } = string.Empty;

    /// <summary>
    /// 音频时长（秒）
    /// </summary>
    public int? AudioDuration { get; set; }

    /// <summary>
    /// 音频文件大小
    /// </summary>
    public int AudioFileSize { get; set; }

    /// <summary>
    /// 音频格式
    /// </summary>
    public string AudioFormat { get; set; } = "mp3";

    /// <summary>
    /// 章节ID
    /// </summary>
    public string ChapterId { get; set; } = string.Empty;

    /// <summary>
    /// 排序索引
    /// </summary>
    public int OrderIndex { get; set; }

    /// <summary>
    /// 是否已发布
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int ViewCount { get; set; }

    /// <summary>
    /// 收藏次数
    /// </summary>
    public int FavoriteCount { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}