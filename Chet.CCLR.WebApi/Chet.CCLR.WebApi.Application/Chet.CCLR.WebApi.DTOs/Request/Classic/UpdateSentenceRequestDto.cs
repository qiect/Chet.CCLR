namespace Chet.CCLR.WebApi.DTOs.Request.Classic;

/// <summary>
/// 更新句子请求DTO
/// </summary>
public class UpdateSentenceRequestDto
{
    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; }

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
    public string? AudioUrl { get; set; }

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
    /// 排序索引
    /// </summary>
    public int OrderIndex { get; set; }

    /// <summary>
    /// 是否已发布
    /// </summary>
    public bool IsPublished { get; set; }
}