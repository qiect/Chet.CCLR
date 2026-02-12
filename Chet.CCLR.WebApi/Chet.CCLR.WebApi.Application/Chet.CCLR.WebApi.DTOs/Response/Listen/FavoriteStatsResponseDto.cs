namespace Chet.CCLR.WebApi.DTOs.Response.Listen;

/// <summary>
/// 收藏统计响应DTO
/// </summary>
public class FavoriteStatsResponseDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 总收藏数
    /// </summary>
    public int TotalFavorites { get; set; }

    /// <summary>
    /// 书籍数量
    /// </summary>
    public int BooksCount { get; set; }

    /// <summary>
    /// 章节数量
    /// </summary>
    public int ChaptersCount { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// 热门收藏
    /// </summary>
    public List<string>? TopFavorites { get; set; }
}