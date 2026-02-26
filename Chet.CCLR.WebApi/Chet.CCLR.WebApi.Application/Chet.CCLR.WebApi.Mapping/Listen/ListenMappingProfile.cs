using AutoMapper;
using Chet.CCLR.WebApi.Domain.Listen;
using Chet.CCLR.WebApi.DTOs.Listen;

namespace Chet.CCLR.WebApi.Mapping.Listen;

/// <summary>
/// 听力学习领域映射配置类
/// </summary>
public class ListenMappingProfile : Profile
{
    /// <summary>
    /// 初始化听力学习领域的映射配置
    /// </summary>
    public ListenMappingProfile()
    {
        // 听读进度相关映射
        CreateMap<UserListenProgress, ProgressResponseDto>();
        CreateMap<UpdateProgressRequestDto, UserListenProgress>();
        
        // 听读记录相关映射
        CreateMap<UserListenRecord, RecordResponseDto>();
        CreateMap<CreateRecordRequestDto, UserListenRecord>();
        CreateMap<UpdateRecordRequestDto, UserListenRecord>();
        
        // 收藏相关映射
        CreateMap<UserFavoriteSentence, FavoriteResponseDto>();
        CreateMap<AddFavoriteRequestDto, UserFavoriteSentence>();
    }
}