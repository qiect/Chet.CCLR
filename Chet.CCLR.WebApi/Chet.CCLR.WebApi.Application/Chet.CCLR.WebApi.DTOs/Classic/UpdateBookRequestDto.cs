namespace Chet.CCLR.WebApi.DTOs.Classic;

/// <summary>
/// 更新书籍请求DTO
/// </summary>
public class UpdateBookRequestDto
{
    /// <summary>
    /// 标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 副标题
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// 朝代
    /// </summary>
    public string? Dynasty { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 封面图片
    /// </summary>
    public string? CoverImage { get; set; }

    /// <summary>
    /// 章节数量
    /// </summary>
    public int TotalChapters { get; set; }

    /// <summary>
    /// 句子数量
    /// </summary>
    public int TotalSentences { get; set; }

    /// <summary>
    /// 难度等级
    /// </summary>
    public byte Level { get; set; }

    /// <summary>
    /// 是否已发布
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// 排序索引
    /// </summary>
    public int OrderIndex { get; set; }
}