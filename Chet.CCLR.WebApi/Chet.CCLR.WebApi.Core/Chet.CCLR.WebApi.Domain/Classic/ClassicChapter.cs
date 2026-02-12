using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chet.CCLR.WebApi.Domain.Classic;

/// <summary>
/// 经典章节实体类，继承自 BaseEntity
/// </summary>
[Table("ClassicChapters")]
public class ClassicChapter : BaseEntity
{
    /// <summary>
    /// 章节标题
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    /// <summary>
    /// 章节描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 章节所属书籍ID
    /// </summary>
    [Required]
    public Guid BookId { get; set; }

    /// <summary>
    /// 本书籍
    /// </summary>
    [ForeignKey(nameof(BookId))]
    public virtual ClassicBook? Book { get; set; }

    /// <summary>
    /// 本章节句子总数
    /// </summary>
    public int TotalSentences { get; set; } = 0;

    /// <summary>
    /// 排序序号
    /// </summary>
    public int OrderIndex { get; set; } = 0;

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool IsPublished { get; set; } = false;

    /// <summary>
    /// 句子集合
    /// </summary>
    public virtual ICollection<ClassicSentence> Sentences { get; set; } = new List<ClassicSentence>();
}