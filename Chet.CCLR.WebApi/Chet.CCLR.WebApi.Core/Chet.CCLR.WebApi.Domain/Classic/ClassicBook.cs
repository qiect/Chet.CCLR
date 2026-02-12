using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chet.CCLR.WebApi.Domain.Classic;

/// <summary>
/// 经典书籍实体类，继承自 BaseEntity
/// </summary>
[Table("ClassicBooks")]
public class ClassicBook : BaseEntity
{
    /// <summary>
    /// 书籍标题
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    /// <summary>
    /// 书籍副标题
    /// </summary>
    [MaxLength(200)]
    public string? Subtitle { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    [MaxLength(100)]
    public string? Author { get; set; }

    /// <summary>
    /// 朝代
    /// </summary>
    [MaxLength(50)]
    public string? Dynasty { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    [MaxLength(50)]
    public string? Category { get; set; }

    /// <summary>
    /// 书籍描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 封面图片URL
    /// </summary>
    public string? CoverImage { get; set; }

    /// <summary>
    /// 总章节数
    /// </summary>
    public int TotalChapters { get; set; } = 0;

    /// <summary>
    /// 总句子数
    /// </summary>
    public int TotalSentences { get; set; } = 0;

    /// <summary>
    /// 难度等级 (1-5)
    /// </summary>
    public byte Level { get; set; } = 1;

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool IsPublished { get; set; } = false;

    /// <summary>
    /// 排序序号
    /// </summary>
    public int OrderIndex { get; set; } = 0;

    /// <summary>
    /// 章节集合
    /// </summary>
    public virtual ICollection<ClassicChapter> Chapters { get; set; } = new List<ClassicChapter>();
}