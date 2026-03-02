using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Craw;
using HtmlAgilityPack;

namespace Chet.CCLR.WebApi.Services.Craw;

/// <summary>
/// 汉程国学爬虫适配器
/// </summary>
public class HanChengCrawlSource : HtmlCrawlSource
{
    public override string SourceId => "hancheng";

    public override async Task<CrawlResult> CrawlCatalogAsync(string catalogUrl, CancellationToken cancellationToken = default)
    {
        var result = new CrawlResult();

        try
        {
            // 获取目录页（该页面已直接包含 81 章列表）
            var catalogHtml = await GetHtmlAsync(catalogUrl, cancellationToken);

            // 检查是否是错误页面
            if (catalogHtml.Contains("429") || catalogHtml.Contains("错误提示"))
            {
                result.Success = false;
                result.Message = "网站反爬虫机制拦截，请稍后再试";
                return result;
            }

            var catalogDoc = new HtmlDocument();
            catalogDoc.LoadHtml(catalogHtml);

            var chapters = new List<CrawlChapter>();

            // 使用正确的 XPath：div[contains(@class,'lunyu_section')]//a
            var chapterNodes = catalogDoc.DocumentNode.SelectNodes("//div[contains(@class,'lunyu_section')]//a");

            if (chapterNodes != null && chapterNodes.Count > 0)
            {
                int index = 1;
                foreach (var node in chapterNodes)
                {
                    var title = node.InnerText.Trim();
                    var href = node.GetAttributeValue("href", "");

                    // 跳过空链接和 JavaScript 链接
                    if (string.IsNullOrWhiteSpace(href) || href.StartsWith("javascript:"))
                    {
                        continue;
                    }

                    // 跳过非章节链接（如返回首页等）
                    if (title.Contains("首页") || title.Contains("返回") || title.Contains("目录"))
                    {
                        continue;
                    }

                    // 处理链接
                    // 首先移除可能重复的域名前缀
                    href = "https:" + href;

                    chapters.Add(new CrawlChapter
                    {
                        Title = title,
                        OrderIndex = index,
                        DetailUrl = href
                    });

                    index++;
                }
            }

            result.Chapters = chapters;
            result.Success = true;
            result.Message = $"目录爬取成功，共{chapters.Count}章";

            // 获取书籍信息
            var bookTitle = catalogDoc.DocumentNode.SelectSingleNode("//title")?.InnerText.Replace("_汉程网", "").Replace("道德经", "").Trim() ?? "道德经";
            var authorNode = catalogDoc.DocumentNode.SelectSingleNode("//meta[@name='author']");
            var author = authorNode?.GetAttributeValue("content", "") ?? "老子";

            result.Books.Add(new CrawlBook
            {
                Title = "道德经",
                Author = "老子",
                Dynasty = "春秋战国",
                Category = "道家经典",
                Description = "《道德经》又称《老子》，是中国古代先秦诸子分家前的一部著作，为其时诸子所共仰。",
                CoverImage = null
            });
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"目录爬取失败：{ex.Message}";
        }

        return result;
    }

    public override async Task<CrawlResult> CrawlChapterAsync(string catalogUrl, string chapterUrl, CancellationToken cancellationToken = default)
    {
        var result = new CrawlResult();

        try
        {

            _httpClient.DefaultRequestHeaders.Referrer = new Uri(catalogUrl);
            var html = await GetHtmlAsync(chapterUrl, cancellationToken);

            // 检查是否是错误页面
            if (html.Contains("429") || html.Contains("错误提示"))
            {
                result.Success = false;
                result.Message = "网站反爬虫机制拦截，请稍后再试";
                return result;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var sentences = new List<CrawlSentence>();

            // 获取原文内容：div[@class='ly_combine']
            var contentNode = doc.DocumentNode.SelectSingleNode("//div[@class='ly_combine']");

            if (contentNode != null)
            {
                // 提取原文：从 span 标签中提取文本，跳过注释和译文
                var spans = contentNode.SelectNodes(".//span[not(contains(@class, 'ly_fuhao'))]");

                if (spans != null)
                {
                    var originalText = new System.Text.StringBuilder();
                    foreach (var span in spans)
                    {
                        var text = span.InnerText.Trim();
                        if (!string.IsNullOrWhiteSpace(text) && !text.Contains("注释") && !text.Contains("译文"))
                        {
                            originalText.Append(text);
                        }
                    }

                    if (originalText.Length > 0)
                    {
                        sentences.Add(new CrawlSentence
                        {
                            Content = originalText.ToString(),
                            OrderIndex = 1
                        });
                    }
                }
            }

            // 获取注释：div[@class='ly_zhushi']
            var zhushiNode = doc.DocumentNode.SelectSingleNode("//div[@class='ly_zhushi']");
            string? zhushi = null;
            if (zhushiNode != null)
            {
                var zhushiContent = zhushiNode.SelectSingleNode(".//div[@class='lunyu_jies']");
                if (zhushiContent != null)
                {
                    // 提取所有注释段落
                    var zhushiParagraphs = zhushiContent.SelectNodes(".//p");
                    if (zhushiParagraphs != null)
                    {
                        var zhushiBuilder = new System.Text.StringBuilder();
                        foreach (var p in zhushiParagraphs)
                        {
                            var text = p.InnerText.Trim();
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                zhushiBuilder.Append(text).Append("\n");
                            }
                        }
                        zhushi = zhushiBuilder.ToString().Trim();
                    }
                }
            }

            // 获取译文：div[@class='ly_yiwen']
            var yiwenNode = doc.DocumentNode.SelectSingleNode("//div[@class='ly_yiwen']");
            string? translation = null;
            if (yiwenNode != null)
            {
                var yiwenContent = yiwenNode.SelectSingleNode(".//div[@class='lunyu_jies']");
                if (yiwenContent != null)
                {
                    var translationParagraphs = yiwenContent.SelectNodes(".//p");
                    if (translationParagraphs != null)
                    {
                        var translationBuilder = new System.Text.StringBuilder();
                        foreach (var p in translationParagraphs)
                        {
                            var text = p.InnerText.Trim();
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                translationBuilder.Append(text).Append("\n");
                            }
                        }
                        translation = translationBuilder.ToString().Trim();
                    }
                }
            }

            // 如果有注释或译文，更新到句子中
            if (sentences.Count > 0)
            {
                var sentence = sentences[0];
                if (!string.IsNullOrWhiteSpace(zhushi))
                {
                    sentence.Note = zhushi;
                }
                if (!string.IsNullOrWhiteSpace(translation))
                {
                    sentence.Translation = translation;
                }
            }

            result.Sentences = sentences;
            result.Success = true;
            result.Message = $"章节爬取成功，共{sentences.Count}句";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"章节爬取失败：{ex.Message}";
        }

        return result;
    }
}
