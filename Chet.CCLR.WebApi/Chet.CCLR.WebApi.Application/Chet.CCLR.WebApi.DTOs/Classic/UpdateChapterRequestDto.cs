namespace Chet.CCLR.WebApi.DTOs.Classic;

/// <summary>
/// 更新章节请求DTO
/// </summary>
public class UpdateChapterRequestDto
{
    /// <summary>
    /// 标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 排序索引
    /// </summary>
    public int OrderIndex { get; set; }

    /// <summary>
    /// 是否已发布
    /// </summary>
    public bool IsPublished { get; set; }
}