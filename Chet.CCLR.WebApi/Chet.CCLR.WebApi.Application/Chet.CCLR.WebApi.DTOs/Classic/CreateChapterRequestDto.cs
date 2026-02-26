namespace Chet.CCLR.WebApi.DTOs.Classic;

/// <summary>
/// 创建章节请求DTO
/// </summary>
public class CreateChapterRequestDto
{
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
    /// 排序索引
    /// </summary>
    public int OrderIndex { get; set; } = 0;

    /// <summary>
    /// 是否已发布
    /// </summary>
    public bool IsPublished { get; set; } = false;
}