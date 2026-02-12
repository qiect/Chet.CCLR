using Chet.CCLR.WebApi.Contracts;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Data;
using Chet.CCLR.WebApi.Data.Repositories;

namespace Chet.CCLR.WebApi.Api.Configurations;

/// <summary>
/// 仓储服务配置类
/// </summary>
public static class RepositoryConfiguration
{
    /// <summary>
    /// 配置仓储服务
    /// </summary>
    /// <param name="services">IServiceCollection实例</param>
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // 注册通用仓储服务
        services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
        
        // 注册用户相关仓储服务
        services.AddScoped<IUserRepository, UserRepository>();
        
        // 注册经典内容相关仓储服务
        services.AddScoped<IClassicBookRepository, ClassicBookRepository>();
        services.AddScoped<IClassicChapterRepository, ClassicChapterRepository>();
        services.AddScoped<IClassicSentenceRepository, ClassicSentenceRepository>();
        
        // 注册听读管理相关仓储服务
        services.AddScoped<IUserListenProgressRepository, UserListenProgressRepository>();
        services.AddScoped<IUserListenRecordRepository, UserListenRecordRepository>();
        services.AddScoped<IUserFavoriteSentenceRepository, UserFavoriteSentenceRepository>();
        
        // 注册配置和日志相关仓储服务
        services.AddScoped<ISystemConfigRepository, SystemConfigRepository>();
        services.AddScoped<IOperationLogRepository, OperationLogRepository>();
    }
}
