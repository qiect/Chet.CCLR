using AutoMapper;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;
using Chet.CCLR.WebApi.Domain.Config;
using Chet.CCLR.WebApi.Domain.Listen;
using Chet.CCLR.WebApi.DTOs;
using Chet.CCLR.WebApi.DTOs.Request.Classic;
using Chet.CCLR.WebApi.DTOs.Request.Config;
using Chet.CCLR.WebApi.DTOs.Request.Listen;
using Chet.CCLR.WebApi.DTOs.Response.Classic;
using Chet.CCLR.WebApi.DTOs.Response.Config;
using Chet.CCLR.WebApi.DTOs.Response.Listen;

namespace Chet.CCLR.WebApi.Mapping;

/// <summary>
/// AutoMapper配置类，用于定义实体和DTO之间的映射关系
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// 初始化映射配置，定义所有实体和DTO之间的映射规则
    /// </summary>
    public MappingProfile()
    {
        // 用户实体到用户DTO的映射
        CreateMap<User, UserDto>();

        // 用户创建DTO到用户实体的映射
        CreateMap<UserCreateDto, User>();

        // 用户更新DTO到用户实体的映射
        CreateMap<UserUpdateDto, User>();

        // 注册DTO到用户实体的映射
        CreateMap<RegisterDto, User>();
        
        // 经典书籍相关映射
        CreateMap<ClassicBook, BookResponseDto>();
        CreateMap<CreateBookRequestDto, ClassicBook>();
        CreateMap<UpdateBookRequestDto, ClassicBook>();
        
        // 经典章节相关映射
        CreateMap<ClassicChapter, ChapterResponseDto>();
        CreateMap<CreateChapterRequestDto, ClassicChapter>();
        CreateMap<UpdateChapterRequestDto, ClassicChapter>();
        
        // 经典句子相关映射
        CreateMap<ClassicSentence, SentenceResponseDto>();
        CreateMap<CreateSentenceRequestDto, ClassicSentence>();
        CreateMap<UpdateSentenceRequestDto, ClassicSentence>();
        
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
        
        // 系统配置相关映射
        CreateMap<SystemConfig, ConfigResponseDto>();
        CreateMap<SetConfigRequestDto, SystemConfig>();
    }
}
