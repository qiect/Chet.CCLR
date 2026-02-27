namespace Chet.CCLR.WebApi.Domain;

/// <summary>
/// 用户实体类，继承自 BaseEntity
/// </summary>
public class User : BaseEntity
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
    /// 订阅时间
    /// </summary>
    public DateTime? SubscribeTime { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 用户状态：1正常 0禁用
    /// </summary>
    public byte Status { get; set; } = 1;

    /// <summary>
    /// 用户名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 用户邮箱，用于登录和通知
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 密码哈希值，使用 BCrypt 算法生成
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// 刷新令牌，用于获取新的访问令牌
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// 刷新令牌过期时间
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
