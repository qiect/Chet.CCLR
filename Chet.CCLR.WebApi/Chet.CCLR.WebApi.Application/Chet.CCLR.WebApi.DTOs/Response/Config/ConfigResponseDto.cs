namespace Chet.CCLR.WebApi.DTOs.Response.Config;

/// <summary>
/// 配置响应DTO
/// </summary>
public class ConfigResponseDto
{
    /// <summary>
    /// 配置ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 配置键
    /// </summary>
    public string ConfigKey { get; set; } = string.Empty;

    /// <summary>
    /// 配置值
    /// </summary>
    public string ConfigValue { get; set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

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