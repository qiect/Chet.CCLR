using AutoMapper;
using Chet.CCLR.WebApi.Domain.Classic;
using Chet.CCLR.WebApi.DTOs.Classic;

namespace Chet.CCLR.WebApi.Mapping.Classic;

/// <summary>
/// 经典文学领域映射配置类
/// </summary>
public class ClassicMappingProfile : Profile
{
    /// <summary>
    /// 初始化经典文学领域的映射配置
    /// </summary>
    public ClassicMappingProfile()
    {
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
    }
}