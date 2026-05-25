using OpenQA.Selenium;

namespace SeleniumTestFramework_DotNetTen.PageObjects;

public class GooglePageObjects
{
    public IWebElement About(IWebDriver driver) { return driver.FindElement(By.XPath("//*[text()='About']")); }
    public IWebElement Store(IWebDriver driver) { return driver.FindElement(By.XPath("//*[text()='Store']")); }
    public IWebElement Gmail(IWebDriver driver) { return driver.FindElement(By.XPath("//*[text()='Gmail']")); }
    public IWebElement Images(IWebDriver driver) { return driver.FindElement(By.XPath("//*[text()='Images']")); }
    public IWebElement Apps(IWebDriver driver) { return driver.FindElement(By.XPath("//*[@title='Google apps']")); }
    public IWebElement SignIn(IWebDriver driver) { return driver.FindElement(By.XPath("//*[@aria-label='Sign in']")); }
    public IWebElement AppsAccount(IWebDriver driver) { return driver.FindElement(By.XPath(
        "//*[@aria-label='Google apps']//*[text()='Account']")); }
}