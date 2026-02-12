using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chet.CCLR.WebApi.Domain.Log;

/// <summary>
/// 操作日志实体类，继承自 BaseEntity
/// </summary>
[Table("OperationLogs")]
public class OperationLog : BaseEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    /// <summary>
    /// 操作类型（play, pause, next, prev, favorite等）
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Operation { get; set; }

    /// <summary>
    /// 目标类型（book, chapter, sentence）
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string TargetType { get; set; }

    /// <summary>
    /// 目标ID
    /// </summary>
    [Required]
    public string TargetId { get; set; }

    /// <summary>
    /// 额外数据（JSON格式）
    /// </summary>
    public string? ExtraData { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// 用户代理
    /// </summary>
    public string? UserAgent { get; set; }
}