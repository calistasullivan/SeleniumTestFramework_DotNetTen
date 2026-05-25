/*using System.Text;
using SeleniumTestFramework_DotNetTen.Framework;
using SeleniumTestFramework_DotNetTen.PageObjects;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTestFramework_DotNetTen.Tests;

[TestFixture]
[Category("Reddit")]
public class TheRedditTests : BaseTest
{
    RedditPageObjects redditPageObjects = new RedditPageObjects();
    public StringBuilder dataWriter = new StringBuilder();
    
    public void GrabRedditFrontPageData()
    {
        for (int i = 1; i <= 25; i++)
        {
            //This xpath may need to be updated at some point if reddit changes or updates their website.
            string upvotes = Driver.FindElement(By.XPath("//*[@id='siteTable']//*[@class='rank' and text()='" + i + "']/../div//*[contains(@class, 'unvoted')]")).GetAttribute("title") ??"";

            if (upvotes == "")
            {
                //Sometimes upvote counts are not displayed (usually if a post is newer)
                upvotes = "Not displayed";
            }
            
            string comments = Driver.FindElement(By.XPath("//*[@id='siteTable']//*[@class='rank' and text()='" + i + "']/..//*[contains(@class, 'comments')]")).Text;
            string title = Driver.FindElement(By.XPath("//*[@id='siteTable']//*[@class='rank' and text()='" + i + "']/..//*[@data-event-action='title']")).Text;
            string author = Driver.FindElement(By.XPath("//*[@id='siteTable']//*[@class='rank' and text()='" + i + "']/..//*[contains(@class, 'author')]")).Text;
            string subreddit = Driver.FindElement(By.XPath("//*[@id='siteTable']//*[@class='rank' and text()='" + i + "']/..//*[contains(@class, 'subreddit')]")).Text;
            string linkInternal = Driver.FindElement(By.XPath("//*[@id='siteTable']//*[@class='rank' and text()='" + i + "']/..//*[contains(@class, 'comments')]")).GetAttribute("href") ??"";       
            string linkExternal = Driver.FindElement(By.XPath("//*[@id='siteTable']//*[@class='rank' and text()='" + i + "']/..//*[@data-event-action='title']")).GetAttribute("href") ??"";

            if (linkInternal == linkExternal)
            {
                linkExternal = "Not applicable - No External Link";
            }
            
            dataWriter.Append(i + "," + upvotes + "," + comments.Replace(" comments", "") + "," + title.Replace(",", " ") 
                              + "," + author + "," + subreddit + "," + linkInternal + "," + linkExternal + "\n");
        }
    }
    
    //Grab the front page of Reddit
    [Test, Order(1)] [Description ("Test 1: Can I scrape the front page of Reddit?")]
    public void RedditReportGenerator()
    {
        string filepath = ReportFolder + "/RedditReport";
        string filename = DateTime + " - Reddit Front Page Posts.csv";
        var writer = new StreamWriter(filename); //Remove this?
        dataWriter.Append("Rank,Upvotes,Comments,Title,Author,Subreddit,Internal Link, External Link (if applicable)" + "\n");
        GrabRedditFrontPageData();
        File.WriteAllText(filepath + "/" + filename, dataWriter.ToString());
        writer.Close();
        Assert.That(File.Exists(filepath + "/" + filename));
        Test.Log(Status.Pass, "File successfully created: " + filename);
    }
    
    [Test, Order(2)] [Description("Test 2: Does the login link bring the user to the login page?")]
    public void LoginLink()
    {
        WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WebDriverWaitTime));
        Assert.That(redditPageObjects.LoginLink(Driver).Displayed, "'Login' link is NOT displayed!");
        Test.Log(Status.Pass, "'Login' link is displayed.");
        wait.Until(ExpectedConditions.ElementToBeClickable(redditPageObjects.LoginLink(Driver))).Click();
        wait.Until(ExpectedConditions.UrlContains(RedditConfig["login"]));
        Assert.That(Driver.Url.Contains(RedditConfig["login"] ??""), "'Login' URL is incorrect!");
        Test.Log(Status.Pass, "'Login' URL is correct.");
        Assert.That(redditPageObjects.EmailUsernameTextbox(Driver).Displayed, "'Email/Username' textbox is NOT displayed!");
        Test.Log(Status.Pass, "'Email/Username' textbox is displayed.");
        Assert.That(redditPageObjects.PasswordTextbox(Driver).Displayed, "'Password' textbox is NOT displayed!");
        Test.Log(Status.Pass, "'Password' textbox is displayed.");
    }
    
}*/