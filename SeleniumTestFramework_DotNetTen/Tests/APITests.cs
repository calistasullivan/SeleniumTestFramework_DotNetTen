using System.Text;
using SeleniumTestFramework_DotNetTen.Framework;
using System.Text.Json;
using AventStack.ExtentReports;

namespace SeleniumTestFramework_DotNetTen.Tests;

[TestFixture]
[Category("API")]
public class TheApiTests : BaseTest
{
    //If you're getting 400 errors, you probably need a paid subscription. This project uses the free tier.
    //I removed the API key from the APITest.config file.
    //Go to NewsAPI.org, get one from there, and put it in the config.file.

    //This isn't the best way to do this, but it's super easy.
    [Test][Description("Test 0: Can I output the result of an API call to a csv file?")]
    public async Task LetsGrabSomeHeadlines_APIReportGenerator()
    {
        string url = $"{EnvConfig["base_url"]}?country=us&category=business&apiKey={EnvConfig["api_key"]}";
        HttpResponseMessage response = await httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {content}");
        Test.Log(Status.Pass, "Received status code 200 - OK!");
        JsonDocument json = JsonDocument.Parse(content);
        JsonElement articles = json.RootElement.GetProperty("articles");
        StringBuilder dataWriter = new StringBuilder();
        dataWriter.AppendLine("Source,Author,Title,Description,URL,PublicationDate,Content");
        foreach (JsonElement article in articles.EnumerateArray())
        {
            string source         = CsvEscape(article.GetProperty("source").GetProperty("name").GetString() ??"");
            string author         = CsvEscape(article.GetProperty("author").GetString() ??"");
            string title          = CsvEscape(article.GetProperty("title").GetString() ??"");
            string description    = CsvEscape(article.GetProperty("description").GetString() ??"");
            string articleUrl     = CsvEscape(article.GetProperty("url").GetString() ??"");
            string publishedAt    = CsvEscape(article.GetProperty("publishedAt").GetString() ??"");
            string articleContent = CsvEscape(article.GetProperty("content").GetString() ??"");
            dataWriter.AppendLine($"{source},{author},{title},{description},{articleUrl},{publishedAt},{articleContent}");
        }
        string filepath = ReportFolder + "/APIReport";
        string filename = DateTime + " - API News Reports (NewsAPI).csv";
        var writer = new StreamWriter(filename);
        File.WriteAllText(filepath + "/" + filename, dataWriter.ToString());
        writer.Close();
        Assert.That(File.Exists(filename));
        Test.Log(Status.Pass, "File successfully created: " + filename);
    }
    
    //This handles unexpected shenanigans
    private string CsvEscape(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        value = value.Replace("\"", "\"\"");
        return $"\"{value}\"";
    }

    [Test][Description("Test 1: Does the API call return a successful status code?")]
    public async Task DoesTheResponseReturnASuccessfulStatusCode()
    {
        string url = $"{EnvConfig["base_url"]}?country=us&category=business&apiKey={EnvConfig["api_key"]}";
        HttpResponseMessage response = await httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(response.IsSuccessStatusCode,
            $"Expected success status code but got {(int)response.StatusCode} {response.StatusCode}");
        Test.Log(Status.Pass, "Received status code 200 - OK!");
    }
    
    [Test][Description("Test 2: Does the API call return a valid JSON?")]
    public async Task DoesTheResponseReturnAValidJson()
    {
        string url = $"{EnvConfig["base_url"]}?country=us&category=business&apiKey={EnvConfig["api_key"]}";
        HttpResponseMessage response = await httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        Assert.DoesNotThrow(() => { JsonDocument.Parse(content); }, "Response body is not valid JSON!");
        Test.Log(Status.Pass, "Response body is a valid JSON.");
    }
    
    [Test][Description("Test 3: Does the API call return articles, total results, and is it in an array format?")]
    public async Task DoesTheResponseContainsArticles()
    {
        string url = $"{EnvConfig["base_url"]}?country=us&category=business&apiKey={EnvConfig["api_key"]}";
        HttpResponseMessage response = await httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        JsonDocument json = JsonDocument.Parse(content);
        JsonElement root = json.RootElement;
        Assert.AreEqual("ok", root.GetProperty("status").GetString(), "Expected status to be 'ok', it is NOT!");
        Test.Log(Status.Pass, "Status code 200 - OK!");
        Assert.IsTrue(root.TryGetProperty("totalResults", out _), "Response missing 'totalResults' field");
        Test.Log(Status.Pass, "Response contains 'totalResults' field.");
        Assert.IsTrue(root.TryGetProperty("articles", out JsonElement articles), "Response missing 'articles' field");
        Test.Log(Status.Pass, "Response contains 'articles' field.");
        Assert.AreEqual(JsonValueKind.Array, articles.ValueKind, "'articles' should be an array, but it is NOT!");
        Test.Log(Status.Pass, "'articles' is an array. (That's good.)");
    }
    
    [Test][Description("Test 4: Is the correct content type being returned?")]
    public async Task DoesTheResponseReturnTheCorrectContentType()
    {
        string url = $"{EnvConfig["base_url"]}?country=us&category=business&apiKey={EnvConfig["api_key"]}";
        HttpResponseMessage response = await httpClient.GetAsync(url);
        string contentType = response.Content.Headers.ContentType?.MediaType ??"";
        Assert.AreEqual("application/json", contentType, $"Expected 'application/json' but got '{contentType}'");
        Test.Log(Status.Pass, "Content type is application/json. (That's good.)");
    }
    
}