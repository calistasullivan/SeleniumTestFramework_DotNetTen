using OpenQA.Selenium;

namespace SeleniumTestFramework_DotNetTen.PageObjects;

public class RedditPageObjects
{
    public IWebElement LoginLink(IWebDriver driver) { return driver.FindElement(By.XPath("//*[contains(@class, 'login-link')]")); }
    public IWebElement EmailUsernameTextbox(IWebDriver driver) { return driver.FindElement(By.XPath("//*[@name='username']")); }
    public IWebElement PasswordTextbox(IWebDriver driver) { return driver.FindElement(By.XPath("//*[@name='password']")); }
    public IWebElement PremiumLink(IWebDriver driver) { return driver.FindElement(By.XPath("//*[contains(@class, 'premium-banner')]")); }
    //This xpath is gross. Fix it later.
    public IWebElement PremiumImg(IWebDriver driver) { return driver.FindElement(
        By.XPath("//*[contains(@title, 'Reddit Premium')]//following-sibling::div/div[1]/div/div[1]/img")); }
}