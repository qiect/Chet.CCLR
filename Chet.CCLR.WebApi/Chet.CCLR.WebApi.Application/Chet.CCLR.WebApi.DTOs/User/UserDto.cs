namespace Chet.CCLR.WebApi.DTOs.User;

/// <summary>
/// 用户数据传输对象，用于返回用户信息给客户端
/// </summary>
public class UserDto
{
    /// <summary>
    /// 用户ID，唯一标识符
    /// </summary>
    public Guid Id { get; set; }

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
    public byte Gender { get; set; }

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
    /// 用户名
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 用户邮箱
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// 用户状态：1正常 0禁用
    /// </summary>
    public byte Status { get; set; }

    /// <summary>
    /// 用户创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 用户信息更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }
}