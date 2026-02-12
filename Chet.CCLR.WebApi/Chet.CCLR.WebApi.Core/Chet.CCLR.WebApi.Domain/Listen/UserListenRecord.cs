using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Chet.CCLR.WebApi.Domain.Classic;

namespace Chet.CCLR.WebApi.Domain.Listen;

/// <summary>
/// 用户听读记录实体类，继承自 BaseEntity
/// </summary>
[Table("UserListenRecord")]
public class UserListenRecord : BaseEntity
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
    public Guid? ChapterId { get; set; }

    /// <summary>
    /// 章节
    /// </summary>
    [ForeignKey(nameof(ChapterId))]
    public virtual ClassicChapter? Chapter { get; set; }

    /// <summary>
    /// 句子ID列表（逗号分隔）
    /// </summary>
    public string? SentenceIds { get; set; }

    /// <summary>
    /// 听读日期
    /// </summary>
    [Required]
    public DateOnly ListenDate { get; set; }

    /// <summary>
    /// 本次听读时长（秒）
    /// </summary>
    public int DurationSec { get; set; } = 0;

    /// <summary>
    /// 本次听读句子数量
    /// </summary>
    public int SentenceCount { get; set; } = 0;

    /// <summary>
    /// 是否为有效听读日（>=180秒）
    /// </summary>
    public bool IsValidDay { get; set; } = false;
}