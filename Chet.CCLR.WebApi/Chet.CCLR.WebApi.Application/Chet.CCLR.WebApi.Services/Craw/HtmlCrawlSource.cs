using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Craw;
using HtmlAgilityPack;
using System.Net;

namespace Chet.CCLR.WebApi.Services.Craw;

/// <summary>
/// HTML 爬虫基类
/// </summary>
public class HtmlCrawlSource : ICrawlSource
{
    private static readonly Random _random = new();
    private static readonly SemaphoreSlim _throttle = new(2); // 最大并发2个

    protected static readonly HttpClient _httpClient;

    static HtmlCrawlSource()
    {
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = true,
            UseCookies = true,
            CookieContainer = new CookieContainer(),
            AutomaticDecompression =
                DecompressionMethods.GZip |
                DecompressionMethods.Deflate |
                DecompressionMethods.Brotli
        };

        _httpClient = new HttpClient(handler);

        _httpClient.Timeout = TimeSpan.FromSeconds(30);

        _httpClient.DefaultRequestHeaders.Accept.ParseAdd(
            "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

        _httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("zh-CN,zh;q=0.9,en;q=0.8");
    }

    protected virtual string GetRandomUserAgent()
    {
        var agents = new[]
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/120.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 Version/17.0 Safari/605.1.15",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 Chrome/119.0.0.0 Safari/537.36"
        };

        return agents[_random.Next(agents.Length)];
    }

    protected async Task<string> GetHtmlAsync(string url, CancellationToken cancellationToken)
    {
        await _throttle.WaitAsync(cancellationToken);

        try
        {
            // 🔹 随机延迟 500~1500ms
            await Task.Delay(_random.Next(500, 1500), cancellationToken);

            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.UserAgent.ParseAdd(GetRandomUserAgent());
            request.Headers.Referrer = new Uri(url);
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Cache-Control", "max-age=0");

            for (int retry = 0; retry < 3; retry++)
            {
                var response = await _httpClient.SendAsync(request, cancellationToken);

                if ((int)response.StatusCode == 429)
                {
                    // 🔥 处理 Retry-After
                    if (response.Headers.TryGetValues("Retry-After", out var values))
                    {
                        var delay = int.TryParse(values.FirstOrDefault(), out var seconds)
                            ? seconds * 1000
                            : 3000;

                        await Task.Delay(delay, cancellationToken);
                    }
                    else
                    {
                        await Task.Delay(3000 + retry * 2000, cancellationToken);
                    }

                    continue;
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync(cancellationToken);
            }

            throw new Exception("多次重试仍然失败");
        }
        finally
        {
            _throttle.Release();
        }
    }

    /// <summary>
    /// 数据源 ID，基类返回空实现
    /// </summary>
    public virtual string SourceId => string.Empty;

    public virtual async Task<CrawlResult> CrawlCatalogAsync(string catalogUrl, CancellationToken cancellationToken = default)
    {
        var result = new CrawlResult();

        try
        {
            var html = await GetHtmlAsync(catalogUrl, cancellationToken);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var chapters = new List<CrawlChapter>();

            var chapterNodes = doc.DocumentNode.SelectNodes("//div[@class='catalog']//a")
                ?? doc.DocumentNode.SelectNodes("//div[@class='booklist']//a")
                ?? doc.DocumentNode.SelectNodes("//ul[@class='chapter']//a");

            if (chapterNodes != null)
            {
                int index = 1;
                foreach (var node in chapterNodes)
                {
                    var title = node.InnerText.Trim();
                    var href = node.GetAttributeValue("href", "");

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
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"目录爬取失败：{ex.Message}";
        }

        return result;
    }

    public virtual async Task<CrawlResult> CrawlChapterAsync(string catalogUrl, string chapterUrl, CancellationToken cancellationToken = default)
    {
        var result = new CrawlResult();

        try
        {
            _httpClient.DefaultRequestHeaders.Referrer = new Uri(catalogUrl);
            var html = await GetHtmlAsync(chapterUrl, cancellationToken);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var sentences = new List<CrawlSentence>();

            var contentNode = doc.DocumentNode.SelectSingleNode("//div[@class='article-content']")
                ?? doc.DocumentNode.SelectSingleNode("//div[@class='content']")
                ?? doc.DocumentNode.SelectSingleNode("//div[@id='article-content']");

            if (contentNode != null)
            {
                HtmlNodeCollection? paragraphs = contentNode.SelectNodes(".//p")
                    ?? contentNode.SelectNodes(".//div");

                if (paragraphs == null)
                {
                    paragraphs = new HtmlNodeCollection(contentNode);
                    foreach (var childNode in contentNode.ChildNodes.Where(n => n.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(n.InnerText.Trim())))
                    {
                        paragraphs.Add(childNode);
                    }
                }

                int index = 1;
                foreach (var para in paragraphs)
                {
                    var text = para.InnerText.Trim();
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        sentences.Add(new CrawlSentence
                        {
                            Content = text,
                            OrderIndex = index
                        });
                        index++;
                    }
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
