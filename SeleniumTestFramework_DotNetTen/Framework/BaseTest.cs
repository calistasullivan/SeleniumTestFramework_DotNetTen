using System.Collections.Specialized;
using System.Drawing;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using Status = AventStack.ExtentReports.Status;
using NUnit.Framework.Interfaces;
using TestContext = NUnit.Framework.TestContext;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTestFramework_DotNetTen.Framework;

public class BaseTest
{
    public IWebDriver Driver = null!;
    public int WebDriverWaitTime = 60;
    public WebDriverWait CreateWait() => new WebDriverWait(Driver, TimeSpan.FromSeconds(WebDriverWaitTime));
    //The reports are put here for easy access during development.
    public string MainReportFolder = Path.Combine(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory)?
        .FullName ?? Environment.CurrentDirectory)?.FullName ?? Environment.CurrentDirectory,
        "ExtentReports/../../ExtentReports"
    );
    public string ReportFolder = "";
    public static NameValueCollection BaseConfig = null!;
    public static NameValueCollection EnvConfig = null!;
    public static NameValueCollection RedditConfig = null!;
    public static NameValueCollection GoogleConfig = null!;
    public static NameValueCollection APIConfig = null!;
    public static string DateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-tt");
    public string ExtentFolder = @"./../ExtentReports/";
    public ExtentReports Extent = new ExtentReports();
    public ExtentTest Test = null!;
    public ExtentSparkReporter HtmlReporter = null!;
    public HttpClient httpClient = new HttpClient(); 
    
    public IWebDriver GetWebDriver()
    {
        switch (BaseConfig["webBrowser"])
        {
            case "Firefox":
            {
                FirefoxOptions options = new FirefoxOptions();
                if (BaseConfig["isHeadless"] == "True")
                {
                    options.AddArguments("--headless");
                    options.AddArgument("--user-agent=Mozilla/5.0 " +
                        "(Windows NT 10.0; Win64; x64; rv:124.0) Gecko/20100101 Firefox/124.0");
                }
                Driver = new FirefoxDriver(options);
                break;
            }
            
            case "Chrome":
            {
                ChromeOptions options = new ChromeOptions();
                if(BaseConfig["isHeadless"] == "True"){options.AddArguments("--headless=new", "--no-sandbox");}
                Driver = new ChromeDriver(options);
                break;
            }
            
            case "MSEdge":
            {
                EdgeOptions options = new EdgeOptions();
                if(BaseConfig["isHeadless"] == "True"){options.AddArguments("--headless");}
                Driver = new EdgeDriver(options);
                break;
            }
            default:
                throw new ArgumentException($"Unsupported browser: {BaseConfig["webBrowser"]}");
        }
        return Driver;
    }
    
    private static string ReportTitle()
    {
        if (BaseConfig["target_env"] == "API")
        {
            return DateTime + " - " + EnvConfig["env"] + " - "  + "API Tests" + " - Automation Report";
        }
        else
        {
            return DateTime + " - " + EnvConfig["env"] + " - "  + BaseConfig["webBrowser"] + " - Automation Report";
        }
    }
    
    private Media GetErrorScreenshot()
    {
        var screenshotName = TestContext.CurrentContext.Test.Name + ".png";
        var screenshotFolder = ReportFolder + "/Screenshots/";
        var fullPath = screenshotFolder + screenshotName;
        Screenshot file = ((ITakesScreenshot)Driver).GetScreenshot();
        file.SaveAsFile(fullPath);
        return MediaEntityBuilder.CreateScreenCaptureFromPath("Screenshots/" + screenshotName).Build();
    }
  
    private NameValueCollection LoadTheConfigs()
    {
        // Load BaseConfig
        BaseConfig = new NameValueCollection();
        var baseXml = new System.Xml.XmlDocument();
        baseXml.Load("Framework/EnvConfigFiles/Base.config");
        var elements = baseXml.DocumentElement?.GetElementsByTagName("add") 
                       ?? throw new InvalidOperationException("Invalid config file: missing root element.");
        foreach (System.Xml.XmlElement element in elements)
        {
            BaseConfig.Add(element.GetAttribute("key"), element.GetAttribute("value"));
        }
        
        //Load Selected Environment Config
        switch (BaseConfig["target_env"])
        {
            //NOTE: The Reddit tests will not work with CI/CD. They block requests from Microsoft Azure's IP ranges.
            case "Reddit":
            {
                RedditConfig = new NameValueCollection();
                var redditXml = new System.Xml.XmlDocument();
                redditXml.Load("Framework/EnvConfigFiles/Reddit.config");
                
                
                if (redditXml.DocumentElement != null)
                {
                    foreach (System.Xml.XmlElement element in redditXml.DocumentElement.GetElementsByTagName("add"))
                    {
                        RedditConfig.Add(element.GetAttribute("key"), element.GetAttribute("value"));
                    }
                }
                EnvConfig = RedditConfig;
                break;
            }
            
            case "Google":
            {
                GoogleConfig = new NameValueCollection();
                var googleXml = new System.Xml.XmlDocument();
                googleXml.Load("Framework/EnvConfigFiles/Google.config");
                if (googleXml.DocumentElement != null)
                {
                    foreach (System.Xml.XmlElement element in googleXml.DocumentElement.GetElementsByTagName("add"))
                    {
                        GoogleConfig.Add(element.GetAttribute("key"), element.GetAttribute("value"));
                    }
                }
                EnvConfig = GoogleConfig;
                break;
            }
            
            case "API":
            {
                APIConfig = new NameValueCollection();
                var apiXml = new System.Xml.XmlDocument();
                apiXml.Load("Framework/EnvConfigFiles/API.config");
                if (apiXml.DocumentElement != null)
                {
                    foreach (System.Xml.XmlElement element in apiXml.DocumentElement.GetElementsByTagName("add"))
                    {
                        APIConfig.Add(element.GetAttribute("key"), element.GetAttribute("value"));
                    }
                }
                
                var envKey = Environment.GetEnvironmentVariable("NEWS_API_KEY");
                if (!string.IsNullOrEmpty(envKey))
                    APIConfig["api_key"] = envKey;
                
                EnvConfig = APIConfig;
                break;
            }
            
        }
        return EnvConfig;
    }
    
    [OneTimeSetUp]
    public void RunBeforeSuite()
    {
        LoadTheConfigs();
        Directory.CreateDirectory(MainReportFolder);
        ReportFolder = MainReportFolder + "/" + ReportTitle();
        Directory.CreateDirectory(ReportFolder);
        Directory.CreateDirectory(ReportFolder + "/Screenshots");
        
        if (BaseConfig["target_env"] == "Reddit")
        {
            Directory.CreateDirectory(ReportFolder + "/RedditReport");
        }
        else if (BaseConfig["target_env"] == "API")
        {
            Directory.CreateDirectory(ReportFolder + "/APIReport");
        }
        
        HtmlReporter = new ExtentSparkReporter(ReportFolder + "/" + ReportTitle() + ".html");
        HtmlReporter.LoadJSONConfig(Environment.CurrentDirectory + "/Framework/extent-config.json");
        Extent.AttachReporter(HtmlReporter);
        Extent.AddTestRunnerLogs(ReportFolder + "/" + ReportTitle() + ".html");
    }
    
    [SetUp]
    public void RunBeforeEachTest()
    {
        Test = Extent.CreateTest(TestContext.CurrentContext.Test.Name);
        if (BaseConfig["target_env"] != "API")
        {
            GetWebDriver();
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Manage().Window.Size = new Size(1920, 1080);
            Driver.Navigate().GoToUrl(EnvConfig["base_url"]
                                      ?? throw new InvalidOperationException(
                                          "base_url is missing or null in EnvConfig."));
        }
        else
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SeleniumTestFramework/1.0");
        }
    }
    
    [TearDown]
    public void RunAfterEachTest()
    {
        string description = TestContext.CurrentContext.Test.Properties
            .Get("Description")?.ToString() ?? string.Empty;        
        switch (TestContext.CurrentContext.Result.Outcome.Status)
        {
            case TestStatus.Failed:
                Test.Log(Status.Fail, TestContext.CurrentContext.Result.StackTrace);
                Test.Log(Status.Fail, TestContext.CurrentContext.Result.Message);
                Test.Log(Status.Fail, description, GetErrorScreenshot());
                break;
            case TestStatus.Passed:
                Test.Log(Status.Pass, description);
                break;
            case TestStatus.Skipped:
                Test.Log(Status.Skip, description);
                break;
            default:
                Test.Log(Status.Info, description);
                break;
        }
        Extent.Flush();
        if (BaseConfig["target_env"] != "API")
        {
            Driver?.Dispose();
        }
        if (BaseConfig["target_env"] == "API")
        {
            httpClient.Dispose();
        }

    }

    [OneTimeTearDown]
    public void RunAfterSuite()
    {
        Extent.Flush();
        if (EnvConfig["target_env"] != "API")
        {
            if (Driver != null)
            {
                try { Driver.Close(); }
                catch { }
                Driver.Quit();
            }
        }
        else
        {
            httpClient.Dispose();
        }
    }
}