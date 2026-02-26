using Microsoft.Extensions.DependencyInjection;
using Chet.CCLR.WebApi.Mapping.Classic;
using Chet.CCLR.WebApi.Mapping.Config;
using Chet.CCLR.WebApi.Mapping.Listen;

namespace Chet.CCLR.WebApi.Mapping;

/// <summary>
/// 映射配置集合类，用于注册所有领域的映射配置
/// </summary>
public static class MappingConfiguration
{
    /// <summary>
    /// 注册所有领域的映射配置
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddAllMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(UserMappingProfile),
            typeof(ClassicMappingProfile),
            typeof(ConfigMappingProfile),
            typeof(ListenMappingProfile),
            typeof(UserMappingProfile)
        );
        return services;
    }
}