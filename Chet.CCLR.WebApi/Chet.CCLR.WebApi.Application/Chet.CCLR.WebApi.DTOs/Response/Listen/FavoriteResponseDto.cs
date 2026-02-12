namespace Chet.CCLR.WebApi.DTOs.Response.Listen;

/// <summary>
/// 收藏响应DTO
/// </summary>
public class FavoriteResponseDto
{
    /// <summary>
    /// 收藏ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 句子ID
    /// </summary>
    public string SentenceId { get; set; } = string.Empty;

    /// <summary>
    /// 笔记/备注
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// 是否公开
    /// </summary>
    public bool IsPublic { get; set; } = false;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}