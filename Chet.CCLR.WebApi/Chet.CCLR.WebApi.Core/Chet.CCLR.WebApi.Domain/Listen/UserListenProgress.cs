using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Chet.CCLR.WebApi.Domain.Classic;

namespace Chet.CCLR.WebApi.Domain.Listen;

/// <summary>
/// 用户听读进度实体类，继承自 BaseEntity
/// </summary>
[Table("UserListenProgress")]
public class UserListenProgress : BaseEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    /// <summary>
    /// 书籍ID
    /// </summary>
    [Required]
    public Guid BookId { get; set; }

    /// <summary>
    /// 书籍
    /// </summary>
    [ForeignKey(nameof(BookId))]
    public virtual ClassicBook? Book { get; set; }

    /// <summary>
    /// 章节ID
    /// </summary>
    [Required]
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 章节
    /// </summary>
    [ForeignKey(nameof(ChapterId))]
    public virtual ClassicChapter? Chapter { get; set; }

    /// <summary>
    /// 句子ID
    /// </summary>
    [Required]
    public Guid SentenceId { get; set; }

    /// <summary>
    /// 当前句子
    /// </summary>
    [ForeignKey(nameof(SentenceId))]
    public virtual ClassicSentence? Sentence { get; set; }

    /// <summary>
    /// 当前句播放到的秒数
    /// </summary>
    public int ProgressSec { get; set; } = 0;

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
    public DateTime LastPlayTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 在句子中的播放百分比
    /// </summary>
    public decimal LastPositionPercent { get; set; } = 0.00m;
}