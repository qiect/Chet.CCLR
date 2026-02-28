using System.ComponentModel.DataAnnotations;

namespace Chet.CCLR.WebApi.DTOs.Auth;

public class WxLoginDto
{
    [Required(ErrorMessage = "code 不能为空")]
    public required string Code { get; set; }

    public string? Nickname { get; set; }

    public string? AvatarUrl { get; set; }

    public byte Gender { get; set; } = 0;

    public string? Country { get; set; }

    public string? Province { get; set; }

    public string? City { get; set; }
}
