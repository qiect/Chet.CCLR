namespace Chet.CCLR.WebApi.DTOs.Classic;

/// <summary>
/// 章节带句子响应DTO
/// </summary>
public class ChapterWithSentencesResponseDto
{
    /// <summary>
    /// 章节信息
    /// </summary>
    public ChapterResponseDto? Chapter { get; set; }

    /// <summary>
    /// 句子列表
    /// </summary>
    public IEnumerable<SentenceResponseDto> Sentences { get; set; } = new List<SentenceResponseDto>();
}