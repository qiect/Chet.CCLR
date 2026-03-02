using Chet.CCLR.WebApi.Contracts;
using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Services;
using Chet.CCLR.WebApi.Services.Auth;
using Chet.CCLR.WebApi.Services.Classic;
using Chet.CCLR.WebApi.Services.Config;
using Chet.CCLR.WebApi.Services.Jwt;
using Chet.CCLR.WebApi.Services.Listen;
using Chet.CCLR.WebApi.Services.User;

namespace Chet.CCLR.WebApi.Api.Configurations;

/// <summary>
/// 业务逻辑服务配置类
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    /// 配置业务逻辑服务
    /// </summary>
    /// <param name="services">IServiceCollection 实例</param>
    public static void ConfigureServices(this IServiceCollection services)
    {
        // 注册 HttpClient 用于调用微信 API
        services.AddHttpClient();
        
        // 认证和 JWT 服务
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        
        // 用户服务
        services.AddScoped<IUserService, UserService>();
        
        // 经典文学服务
        services.AddScoped<IClassicBookService, ClassicBookService>();
        services.AddScoped<IClassicChapterService, ClassicChapterService>();
        services.AddScoped<IClassicSentenceService, ClassicSentenceService>();
        
        // 配置服务
        services.AddScoped<ISystemConfigService, SystemConfigService>();
        
        // 听力学习服务
        services.AddScoped<IUserFavoriteSentenceService, UserFavoriteSentenceService>();
        services.AddScoped<IUserListenProgressService, UserListenProgressService>();
        services.AddScoped<IUserListenRecordService, UserListenRecordService>();
        
        // 爬虫服务
        services.AddScoped<ICrawlService, CrawlService>();
    }
}
