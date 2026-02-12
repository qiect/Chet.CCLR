using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chet.CCLR.WebApi.Domain.Config;

/// <summary>
/// 系统配置实体类，继承自 BaseEntity
/// </summary>
[Table("SystemConfigs")]
public class SystemConfig : BaseEntity
{
    /// <summary>
    /// 配置键
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string ConfigKey { get; set; }

    /// <summary>
    /// 配置值
    /// </summary>
    [Required]
    public string ConfigValue { get; set; }

    /// <summary>
    /// 配置描述
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// 是否公开（前端可获取）
    /// </summary>
    public bool IsPublic { get; set; } = false;
}