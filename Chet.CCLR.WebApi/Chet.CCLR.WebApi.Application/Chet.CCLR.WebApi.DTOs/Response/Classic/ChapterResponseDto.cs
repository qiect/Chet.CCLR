namespace Chet.CCLR.WebApi.DTOs.Response.Classic;

/// <summary>
/// 章节响应DTO
/// </summary>
public class ChapterResponseDto
{
    /// <summary>
    /// 章节ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 书籍ID
    /// </summary>
    public string BookId { get; set; } = string.Empty;

    /// <summary>
    /// 句子数量
    /// </summary>
    public int TotalSentences { get; set; }

    /// <summary>
    /// 排序索引
    /// </summary>
    public int OrderIndex { get; set; }

    /// <summary>
    /// 是否已发布
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}