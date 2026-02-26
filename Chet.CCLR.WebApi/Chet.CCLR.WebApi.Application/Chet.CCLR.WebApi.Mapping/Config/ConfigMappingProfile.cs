using AutoMapper;
using Chet.CCLR.WebApi.Domain.Config;
using Chet.CCLR.WebApi.DTOs.Config;

namespace Chet.CCLR.WebApi.Mapping.Config;

/// <summary>
/// 配置领域映射配置类
/// </summary>
public class ConfigMappingProfile : Profile
{
    /// <summary>
    /// 初始化配置领域的映射配置
    /// </summary>
    public ConfigMappingProfile()
    {
        // 系统配置相关映射
        CreateMap<SystemConfig, ConfigResponseDto>();
        CreateMap<SetConfigRequestDto, SystemConfig>();
    }
}