using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Chet.CCLR.WebApi.Domain.Classic;

namespace Chet.CCLR.WebApi.Domain.Listen;

/// <summary>
/// 用户收藏句子实体类，继承自 BaseEntity
/// </summary>
[Table("UserFavoriteSentences")]
public class UserFavoriteSentence : BaseEntity
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
    /// 句子ID
    /// </summary>
    [Required]
    public Guid SentenceId { get; set; }

    /// <summary>
    /// 收藏的句子（导航属性）
    /// </summary>
    [ForeignKey(nameof(SentenceId))]
    public virtual ClassicSentence? Sentence { get; set; }

    /// <summary>
    /// 用户备注
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// 是否公开
    /// </summary>
    public bool IsPublic { get; set; } = false;

    /// <summary>
    /// 排序序号
    /// </summary>
    public int SortOrder { get; set; } = 0;
}