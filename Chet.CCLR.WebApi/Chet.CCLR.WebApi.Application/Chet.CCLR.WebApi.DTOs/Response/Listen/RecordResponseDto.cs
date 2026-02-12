namespace Chet.CCLR.WebApi.DTOs.Response.Listen;

/// <summary>
/// 记录响应DTO
/// </summary>
public class RecordResponseDto
{
    /// <summary>
    /// 记录ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 听读日期
    /// </summary>
    public DateOnly ListenDate { get; set; }

    /// <summary>
    /// 持续时间（秒）
    /// </summary>
    public int DurationSeconds { get; set; }

    /// <summary>
    /// 句子ID列表
    /// </summary>
    public List<string> SentenceIds { get; set; } = new();

    /// <summary>
    /// 书籍ID
    /// </summary>
    public string BookId { get; set; } = string.Empty;

    /// <summary>
    /// 章节ID
    /// </summary>
    public string ChapterId { get; set; } = string.Empty;

    /// <summary>
    /// 完成的句子数量
    /// </summary>
    public int CompletedSentences { get; set; }

    /// <summary>
    /// 学习进度百分比
    /// </summary>
    public decimal ProgressPercentage { get; set; }

    /// <summary>
    /// 学习心得
    /// </summary>
    public string? Reflection { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}