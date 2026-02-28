using Chet.CCLR.WebApi.Contracts;
using Chet.CCLR.WebApi.DTOs;
using Chet.CCLR.WebApi.DTOs.Auth;
using Chet.CCLR.WebApi.DTOs.User;
using Chet.CCLR.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 认证控制器，处理用户注册、登录和令牌刷新请求
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("提供用户认证相关的API接口，包括注册、登录和令牌刷新")]
public class AuthController : ControllerBase
{
    /// <summary>
    /// 认证服务，用于处理认证相关的业务逻辑
    /// </summary>
    private readonly IAuthService _authService;

    /// <summary>
    /// 日志记录器，用于记录控制器操作日志
    /// </summary>
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="authService">认证服务</param>
    /// <param name="logger">日志记录器</param>
    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// 用户注册接口
    /// </summary>
    /// <param name="registerDto">注册信息DTO，包含用户邮箱、密码和名称</param>
    /// <returns>注册结果</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/Auth/register
    ///     {
    ///         "email": "user@example.com",
    ///         "password": "SecurePassword123!",
    ///         "name": "John Doe"
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 201 Created
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "User registered successfully",
    ///         "statusCode": 201
    ///     }
    /// </remarks>
    /// <response code="201">注册成功</response>
    /// <response code="400">注册失败，邮箱已存在或输入无效</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        _logger.LogInformation("User registration attempt with email: {Email}", registerDto.Email);
        await _authService.RegisterAsync(registerDto);
        return Created("", ApiResponse.Ok(null, "User registered successfully", StatusCodes.Status201Created));
    }

    /// <summary>
    /// 用户登录接口
    /// </summary>
    /// <param name="loginDto">登录信息DTO，包含用户邮箱和密码</param>
    /// <returns>登录成功返回JWT令牌，失败返回401状态码</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/Auth/login
    ///     {
    ///         "email": "user@example.com",
    ///         "password": "SecurePassword123!"
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///             "refreshToken": "dGVzdHJlZnNlcnZpY2U="
    ///         },
    ///         "message": "Login successful",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">登录成功，返回JWT令牌</response>
    /// <response code="401">登录失败，邮箱或密码错误</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        _logger.LogInformation("User login attempt with email: {Email}", loginDto.Email);
        var token = await _authService.LoginAsync(loginDto);
        return Ok(ApiResponse.Ok(token, "Login successful"));
    }

    /// <summary>
    /// 微信登录接口
    /// </summary>
    /// <param name="wxLoginDto">微信登录信息DTO，包含code和用户信息</param>
    /// <returns>登录成功返回用户信息和JWT令牌</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/Auth/wx-login
    ///     {
    ///         "code": "011234567890abcdef",
    ///         "nickname": "微信用户",
    ///         "avatarUrl": "https://example.com/avatar.png",
    ///         "gender": 1,
    ///         "country": "中国",
    ///         "province": "广东",
    ///         "city": "深圳"
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "id": "guid",
    ///             "wxOpenid": "011234567890abcdef",
    ///             "nickname": "微信用户",
    ///             "avatarUrl": "https://example.com/avatar.png",
    ///             "gender": 1,
    ///             "country": "中国",
    ///             "province": "广东",
    ///             "city": "深圳",
    ///             "name": "微信用户",
    ///             "email": "011234567890abcdef@wechat.com",
    ///             "status": 1
    ///         },
    ///         "message": "WeChat login successful",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">登录成功，返回用户信息</response>
    /// <response code="400">登录失败，输入无效</response>
    [HttpPost("wx-login")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> WxLogin(WxLoginDto wxLoginDto)
    {
        _logger.LogInformation("WeChat login attempt with code: {Code}", wxLoginDto.Code);
        var token = await _authService.WxLoginAsync(wxLoginDto);
        return Ok(ApiResponse.Ok(token, "WeChat login successful"));
    }

    /// <summary>
    /// 刷新令牌接口
    /// </summary>
    /// <param name="refreshTokenDto">刷新令牌信息DTO，包含访问令牌和刷新令牌</param>
    /// <returns>刷新成功返回新的JWT令牌，失败返回401状态码</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/Auth/refresh-token
    ///     {
    ///         "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///         "refreshToken": "dGVzdHJlZnNlcnZpY2U="
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///             "refreshToken": "dGVzdHJlZnNlcnZpY2U="
    ///         },
    ///         "message": "Token refreshed successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">令牌刷新成功，返回新的JWT令牌</response>
    /// <response code="401">令牌刷新失败，刷新令牌无效或已过期</response>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        _logger.LogInformation("Token refresh attempt");
        var token = await _authService.RefreshTokenAsync(refreshTokenDto);
        return Ok(ApiResponse.Ok(token, "Token refreshed successfully"));
    }
}
