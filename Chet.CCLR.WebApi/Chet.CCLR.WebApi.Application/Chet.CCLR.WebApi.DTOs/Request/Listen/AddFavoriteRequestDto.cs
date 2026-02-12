namespace Chet.CCLR.WebApi.DTOs.Request.Listen;

/// <summary>
/// 添加收藏请求DTO
/// </summary>
public class AddFavoriteRequestDto
{
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
}