namespace Chet.CCLR.WebApi.DTOs.Response.Listen;

/// <summary>
/// 学习统计响应DTO
/// </summary>
public class LearningStatsResponseDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 总书籍数
    /// </summary>
    public int TotalBooks { get; set; }

    /// <summary>
    /// 平均进度
    /// </summary>
    public decimal AverageProgress { get; set; }

    /// <summary>
    /// 总学习小时数
    /// </summary>
    public int TotalHours { get; set; }

    /// <summary>
    /// 当前连续天数
    /// </summary>
    public int CurrentStreak { get; set; }

    /// <summary>
    /// 最长连续天数
    /// </summary>
    public int LongestStreak { get; set; }

    /// <summary>
    /// 总学习天数
    /// </summary>
    public int TotalStudyDays { get; set; }

    /// <summary>
    /// 总听读句子数
    /// </summary>
    public int TotalSentences { get; set; }

    /// <summary>
    /// 总收藏数
    /// </summary>
    public int TotalFavorites { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}