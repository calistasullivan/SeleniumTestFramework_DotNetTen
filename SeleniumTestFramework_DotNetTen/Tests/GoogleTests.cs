using SeleniumTestFramework_DotNetTen.Framework;
using SeleniumTestFramework_DotNetTen.PageObjects;
using AventStack.ExtentReports;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTestFramework_DotNetTen.Tests;

[TestFixture]
[Category("Google")]
public class Tests : BaseTest
{
    public GooglePageObjects googlePageObjects = new GooglePageObjects();
    
    [Test, Order(1)] [Description ("Test 1: Does the 'About' link bring the user to the 'About' page?")]
    public void DoesTheAboutLinkWork()
    {
        WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WebDriverWaitTime));
        Assert.That(googlePageObjects.About(Driver).Displayed, "'About' link is NOT displayed!");
        Test.Log(Status.Pass, "'About' link is displayed.");
        wait.Until(ExpectedConditions.ElementToBeClickable(googlePageObjects.About(Driver))).Click();
        wait.Until(ExpectedConditions.UrlContains(GoogleConfig["about_url"]));
        Assert.That(Driver.Url.ToString().Contains(GoogleConfig["about_url"] ??""), "'About' URL is incorrect!");
        Test.Log(Status.Pass, "'About' URL is correct.");
    }

    [Test, Order(2)][Description("Test 2: Does the 'Store' link bring the user to the 'Store' page?")]
    public void DoesStoreLinkWork()
    {
        WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WebDriverWaitTime));
        Assert.That(googlePageObjects.Store(Driver).Displayed, "'Store' link is NOT displayed!");
        Test.Log(Status.Pass, "'Store' link is displayed.");
        wait.Until(ExpectedConditions.ElementToBeClickable(googlePageObjects.Store(Driver))).Click();
        wait.Until(ExpectedConditions.UrlContains(GoogleConfig["store_url"]));
        Assert.That(Driver.Url.ToString().Contains(GoogleConfig["store_url"] ??""), "'Store' URL is incorrect!");
        Test.Log(Status.Pass, "'Store' URL is correct.");
    }

    [Test, Order(3)][Description("Test 3: Does the 'Gmail' link bring the user to the 'Gmail' page?")]
    public void DoesGmailLinkWork()
    {
        WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WebDriverWaitTime));
        Assert.That(googlePageObjects.Gmail(Driver).Displayed, "'Gmail' link is NOT displayed.");
        Test.Log(Status.Pass, "'Gmail' link is displayed.");
        wait.Until(ExpectedConditions.ElementToBeClickable(googlePageObjects.Gmail(Driver))).Click();
        try { wait.Until(ExpectedConditions.UrlContains(GoogleConfig["logged_in_gmail_url"])); }
        catch { }
        try { wait.Until(ExpectedConditions.UrlContains(GoogleConfig["not_logged_in_gmail_url"])); }
        catch { }
        Assert.That(Driver.Url.Contains(GoogleConfig["logged_in_gmail_url"] ??"") ||
                    Driver.Url.Contains(GoogleConfig["not_logged_in_gmail_url"] ??""),
                    "'Gmail' URL is incorrect!");
        Test.Log(Status.Pass, "'Gmail' URL is correct.");
    }

    [Test, Order(4)][Description("Test 4: Does the 'Images' link bring the user to the 'Images' page?")]
    public void DoesImagesLinkWork()
    {
        WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WebDriverWaitTime));
        Assert.That(googlePageObjects.Images(Driver).Displayed, "'Images' link is NOT displayed.");
        Test.Log(Status.Pass, "'Images' link is displayed.");
        wait.Until(ExpectedConditions.ElementToBeClickable(googlePageObjects.Images(Driver))).Click();
        wait.Until(ExpectedConditions.UrlContains(GoogleConfig["images_url"] ?? ""));
        Assert.That(Driver.Url.Contains(GoogleConfig["images_url"] ??""), "'Images' URL is incorrect!");
        Test.Log(Status.Pass, "'Images' URL is correct.");
    }

    //This is currently set to fail on purpose (As an example of a fail on the Extent Report)
    //Commented out for CI/CD testing
    /*[Test, Order(5)][Description("Test 5: Does the 'Apps' link open the 'Apps' modal? " +
            "Note: This test is set to fail so you can see error messaging and screenshots.")]
    public void FailTest_DoesAppsLinkWork()
    {
        WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WebDriverWaitTime));
        Assert.That(googlePageObjects.Apps(Driver).Displayed, "'Apps' link is NOT displayed.");
        Test.Log(Status.Pass, "'Apps' link is displayed.");
        wait.Until(ExpectedConditions.ElementToBeClickable(googlePageObjects.Apps(Driver))).Click();
        Driver.SwitchTo().ActiveElement();
        Assert.That(googlePageObjects.AppsAccount(Driver).Displayed, "I put an intentional fail here. You're welcome.");
        Test.Log(Status.Pass, "This should not appear in report because it failed.");
    }*/

    [Test, Order(6)][Description("Test 6: Does the 'Sign In' button bring the user to the 'Sign In' page?")]
    public void DoesSignInButtonWork()
    {
        WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WebDriverWaitTime));
        Assert.That(googlePageObjects.SignIn(Driver).Displayed, "'Sign In Button' link is NOT displayed.");
        Test.Log(Status.Pass, "'Sign In Button' link is displayed.");
        wait.Until(ExpectedConditions.ElementToBeClickable(googlePageObjects.SignIn(Driver))).Click();
        Assert.That(Driver.Url.Contains(GoogleConfig["account_url"] ??""), "'Account' URL is incorrect!");
        Test.Log(Status.Pass, "Account text is displayed.");
    }

    [Test, Order(7)][Description("Test 7: This test is set to be skipped.")]
    public void SkipTestExample()
    {
        Test.Log(Status.Skip, "This is an example of a skipped test.");
    }

}