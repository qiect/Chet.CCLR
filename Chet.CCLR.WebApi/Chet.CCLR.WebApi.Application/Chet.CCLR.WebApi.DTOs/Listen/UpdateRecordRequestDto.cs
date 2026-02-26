namespace Chet.CCLR.WebApi.DTOs.Listen;

/// <summary>
/// 更新记录请求DTO
/// </summary>
public class UpdateRecordRequestDto
{
    /// <summary>
    /// 持续时间（秒）
    /// </summary>
    public int DurationSeconds { get; set; }

    /// <summary>
    /// 句子ID列表
    /// </summary>
    public List<string> SentenceIds { get; set; } = new();

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
}