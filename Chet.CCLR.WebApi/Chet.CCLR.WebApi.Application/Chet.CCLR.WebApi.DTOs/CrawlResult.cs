
namespace Chet.CCLR.WebApi.DTOs;

/// <summary>
/// 爬取结果
/// </summary>
public class CrawlResult
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 书籍列表
    /// </summary>
    public List<CrawlBook> Books { get; set; } = new();

    /// <summary>
    /// 章节列表
    /// </summary>
    public List<CrawlChapter> Chapters { get; set; } = new();

    /// <summary>
    /// 句子列表
    /// </summary>
    public List<CrawlSentence> Sentences { get; set; } = new();
}

/// <summary>
/// 爬取的书籍信息
/// </summary>
public class CrawlBook
{
    /// <summary>
    /// 书名
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 作者
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// 朝代
    /// </summary>
    public string? Dynasty { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 封面图片
    /// </summary>
    public string? CoverImage { get; set; }
}

/// <summary>
/// 爬取的章节信息
/// </summary>
public class CrawlChapter
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 序号
    /// </summary>
    public int OrderIndex { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 详情页URL
    /// </summary>
    public string? DetailUrl { get; set; }
}

/// <summary>
/// 爬取的句子信息
/// </summary>
public class CrawlSentence
{
    /// <summary>
    /// 原文
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 拼音
    /// </summary>
    public string? Pinyin { get; set; }

    /// <summary>
    /// 注释
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// 译文
    /// </summary>
    public string? Translation { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public int OrderIndex { get; set; }
}
