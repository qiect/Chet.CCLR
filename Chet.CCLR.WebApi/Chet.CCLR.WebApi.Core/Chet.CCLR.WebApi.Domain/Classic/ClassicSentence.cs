using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Chet.CCLR.WebApi.Domain.Listen;

namespace Chet.CCLR.WebApi.Domain.Classic;

/// <summary>
/// 经典句子实体类，继承自 BaseEntity
/// </summary>
[Table("ClassicSentences")]
public class ClassicSentence : BaseEntity
{
    /// <summary>
    /// 句子内容（原文）
    /// </summary>
    [Required]
    public string Content { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    public string? Pinyin { get; set; }

    /// <summary>
    /// 注释
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// 译文
    /// </summary>
    public string? Translation { get; set; }

    /// <summary>
    /// 音频文件URL
    /// </summary>
    [Required]
    public string AudioUrl { get; set; }

    /// <summary>
    /// 音频时长（秒）
    /// </summary>
    public int? AudioDuration { get; set; }

    /// <summary>
    /// 音频文件大小（字节）
    /// </summary>
    public int AudioFileSize { get; set; } = 0;

    /// <summary>
    /// 音频格式
    /// </summary>
    [MaxLength(10)]
    public string AudioFormat { get; set; } = "mp3";

    /// <summary>
    /// 章节ID
    /// </summary>
    [Required]
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 所属章节
    /// </summary>
    [ForeignKey(nameof(ChapterId))]
    public virtual ClassicChapter? Chapter { get; set; }

    /// <summary>
    /// 排序序号
    /// </summary>
    public int OrderIndex { get; set; } = 0;

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool IsPublished { get; set; } = true;

    /// <summary>
    /// 查看次数
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// 收藏次数
    /// </summary>
    public int FavoriteCount { get; set; } = 0;

    /// <summary>
    /// 用户收藏记录集合（导航属性）
    /// </summary>
    public virtual ICollection<UserFavoriteSentence> FavoriteRecords { get; set; } = new List<UserFavoriteSentence>();
}