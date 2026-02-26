namespace Chet.CCLR.WebApi.DTOs.Listen;

/// <summary>
/// 进度响应DTO
/// </summary>
public class ProgressResponseDto
{
    /// <summary>
    /// 进度ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 书籍ID
    /// </summary>
    public string BookId { get; set; } = string.Empty;

    /// <summary>
    /// 章节ID
    /// </summary>
    public string ChapterId { get; set; } = string.Empty;

    /// <summary>
    /// 句子ID
    /// </summary>
    public string SentenceId { get; set; } = string.Empty;

    /// <summary>
    /// 当前播放位置（秒）
    /// </summary>
    public int ProgressSec { get; set; }

    /// <summary>
    /// 播放速度
    /// </summary>
    public decimal PlaySpeed { get; set; } = 1.0m;

    /// <summary>
    /// 是否自动滚动
    /// </summary>
    public bool AutoScroll { get; set; } = true;

    /// <summary>
    /// 是否显示拼音
    /// </summary>
    public bool ShowPinyin { get; set; } = true;

    /// <summary>
    /// 最后播放时间
    /// </summary>
    public DateTime LastPlayTime { get; set; }

    /// <summary>
    /// 最后位置百分比
    /// </summary>
    public decimal LastPositionPercent { get; set; } = 0.00m;

    /// <summary>
    /// 是否完成
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}