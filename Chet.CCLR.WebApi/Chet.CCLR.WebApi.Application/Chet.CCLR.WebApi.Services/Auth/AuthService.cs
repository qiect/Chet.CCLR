using AutoMapper;
using Chet.CCLR.WebApi.Configuration;
using Chet.CCLR.WebApi.Contracts;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.DTOs.User;
using Chet.CCLR.WebApi.DTOs.Auth;
using Chet.CCLR.WebApi.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using static BCrypt.Net.BCrypt;

namespace Chet.CCLR.WebApi.Services.Auth;

/// <summary>
/// 认证服务实现类，实现了 IAuthService 接口
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userRepository">用户仓储</param>
    /// <param name="jwtService">JWT服务</param>
    /// <param name="mapper">对象映射器</param>
    /// <param name="logger">日志记录器</param>
    /// <param name="appSettings">应用程序配置</param>
    /// <param name="httpClient">HTTP客户端</param>
    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtService,
        IMapper mapper,
        ILogger<AuthService> logger,
        AppSettings appSettings,
        HttpClient httpClient)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
        _logger = logger;
        _appSettings = appSettings;
        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<JwtTokenDto> LoginAsync(LoginDto loginDto)
    {
        _logger.LogInformation("User login attempt: {Email}", loginDto.Email);

        // 根据邮箱获取用户
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        // 验证用户是否存在且密码正确
        if (user == null || !Verify(loginDto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Invalid login attempt: {Email}", loginDto.Email);
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // 生成访问令牌和刷新令牌
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // 更新用户的刷新令牌和过期时间
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_appSettings.Jwt?.RefreshTokenExpirationDays ?? 7);
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("User login successful: {Email}", loginDto.Email);

        // 返回令牌
        return new JwtTokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <inheritdoc />
    public async Task RegisterAsync(RegisterDto registerDto)
    {
        _logger.LogInformation("User registration attempt: {Email}", registerDto.Email);

        // 检查邮箱是否已存在
        var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User registration failed: Email already exists: {Email}", registerDto.Email);
            throw new BadRequestException("Email already exists");
        }

        // 将注册DTO映射为用户实体
        var user = _mapper.Map<Domain.User>(registerDto);
        // 对密码进行哈希处理
        user.PasswordHash = HashPassword(registerDto.Password);

        // 添加用户到数据库
        await _userRepository.AddAsync(user);

        _logger.LogInformation("User registration successful: {Email}", registerDto.Email);
    }

    /// <inheritdoc />
    public async Task<JwtTokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        _logger.LogInformation("Refresh token attempt");

        // 使用JWT服务刷新令牌
        var token = await _jwtService.RefreshTokenAsync(refreshTokenDto.AccessToken, refreshTokenDto.RefreshToken);
        return token;
    }

    /// <inheritdoc />
    public async Task<WxLoginResponseDto> WxLoginAsync(WxLoginDto wxLoginDto)
    {
        _logger.LogInformation("WeChat login attempt");

        // 使用 code 换取 openid
        var wxOpenid = await GetOpenIdAsync(wxLoginDto.Code);
        if (string.IsNullOrEmpty(wxOpenid))
        {
            _logger.LogError("Failed to get openid from WeChat");
            throw new BadRequestException("Failed to get openid from WeChat");
        }

        var user = await _userRepository.GetByWxOpenidAsync(wxOpenid);

        if (user == null)
        {
            user = new Domain.User
            {
                WxOpenid = wxOpenid,
                Nickname = wxLoginDto.Nickname,
                AvatarUrl = wxLoginDto.AvatarUrl,
                Gender = wxLoginDto.Gender,
                Country = wxLoginDto.Country,
                Province = wxLoginDto.Province,
                City = wxLoginDto.City,
                Name = wxLoginDto.Nickname ?? "WeChatUser",
                Email = $"{wxOpenid}@wechat.com",
                PasswordHash = string.Empty,
                Status = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _userRepository.AddAsync(user);
            _logger.LogInformation("New WeChat user created: {Email}", user.Email);
        }
        else
        {
            user.Nickname = wxLoginDto.Nickname ?? user.Nickname;
            user.AvatarUrl = wxLoginDto.AvatarUrl ?? user.AvatarUrl;
            user.Gender = wxLoginDto.Gender > 0 ? wxLoginDto.Gender : user.Gender;
            user.Country = wxLoginDto.Country ?? user.Country;
            user.Province = wxLoginDto.Province ?? user.Province;
            user.City = wxLoginDto.City ?? user.City;
            user.UpdatedAt = DateTime.Now;
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation("WeChat user updated: {Email}", user.Email);
        }

        user.LastLoginTime = DateTime.Now;
        await _userRepository.UpdateAsync(user);

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_appSettings.Jwt?.RefreshTokenExpirationDays ?? 7);
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("WeChat login successful: {Email}", user.Email);

        var userDto = _mapper.Map<UserDto>(user);
        return new WxLoginResponseDto
        {
            User = userDto,
            Token = new JwtTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            }
        };
    }

    /// <summary>
    /// 使用 code 换取 openid
    /// </summary>
    /// <param name="code">微信登录凭证</param>
    /// <returns>openid</returns>
    private async Task<string?> GetOpenIdAsync(string code)
    {
        try
        {
            var appId = _appSettings?.WeChat?.AppId;
            var appSecret = _appSettings?.WeChat?.AppSecret;

            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appSecret))
            {
                _logger.LogError("WeChat AppId or AppSecret is not configured");
                return null;
            }

            var url = $"https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={appSecret}&js_code={code}&grant_type=authorization_code";
            
            _logger.LogInformation("Requesting WeChat API: {Url}", url);
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("WeChat API request failed with status code: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("WeChat API response: {Content}", content);

            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            if (root.TryGetProperty("openid", out var openidElement))
            {
                return openidElement.GetString();
            }

            if (root.TryGetProperty("errcode", out var errcodeElement))
            {
                var errcode = errcodeElement.GetInt32();
                var errmsg = root.GetProperty("errmsg").GetString();
                _logger.LogError("WeChat API error: {ErrCode} - {ErrMsg}", errcode, errmsg);
                return null;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting openid from WeChat");
            return null;
        }
    }
}
