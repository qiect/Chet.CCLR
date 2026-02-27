using System.ComponentModel.DataAnnotations;

namespace Chet.CCLR.WebApi.DTOs.User;

/// <summary>
/// 用户创建数据传输对象，用于接收创建用户的请求
/// </summary>
public class UserCreateDto
{
    /// <summary>
    /// 微信开放ID，用于微信登录
    /// </summary>
    public string? WxOpenid { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 用户头像URL
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// 性别：0未知 1男 2女
    /// </summary>
    public byte Gender { get; set; } = 0;

    /// <summary>
    /// 国家
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// 省份
    /// </summary>
    public string? Province { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 语言
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// 用户名，用于标识用户
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    [MaxLength(100, ErrorMessage = "用户名长度不能超过100个字符")]
    public required string Name { get; set; }

    /// <summary>
    /// 用户邮箱，用于登录和通知
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "请输入有效的邮箱地址")]
    [MaxLength(255, ErrorMessage = "邮箱长度不能超过255个字符")]
    public required string Email { get; set; }

    /// <summary>
    /// 用户密码，用于登录认证
    /// </summary>
    [Required(ErrorMessage = "密码不能为空")]
    [MinLength(6, ErrorMessage = "密码长度不能少于6个字符")]
    public required string Password { get; set; }
}