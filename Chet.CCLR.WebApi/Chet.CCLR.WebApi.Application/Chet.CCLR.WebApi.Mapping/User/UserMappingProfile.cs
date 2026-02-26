using AutoMapper;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.DTOs.User;

namespace Chet.CCLR.WebApi.Mapping;

/// <summary>
/// 用户领域映射配置类
/// </summary>
public class UserMappingProfile : Profile
{
    /// <summary>
    /// 初始化用户领域的映射配置
    /// </summary>
    public UserMappingProfile()
    {
        // 用户实体到用户DTO的映射
        CreateMap<User, UserDto>();

        // 用户创建DTO到用户实体的映射
        CreateMap<UserCreateDto, User>();

        // 用户更新DTO到用户实体的映射
        CreateMap<UserUpdateDto, User>();

        // 注册DTO到用户实体的映射
        CreateMap<RegisterDto, User>();
    }
}