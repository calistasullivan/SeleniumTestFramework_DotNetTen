Selenium .Net 10 Test Automation Framework:===============================================

A Selenium Automation Suite created and maintained by Calista Sullivan
This is an updated version of my previous .net 4.8 project, now using:

    C# .NET 10 Framework
    NUnit
    Selenium
    Extent Reports 5.0.4

Basic Project Structure: 
Framework - Config files, General Setup, Reporting (when not using CI/CD), Misc stuff (like test data generators)
PageObjects - Page locators/Xpaths/Selectors. They are organized into their own classes by page (or component part).
Tests - This is where the tests, and their actions live. Organized the same way PageObjects are.

Features to be added and/or moved from my old project to this one, aka, the Roadmap=========

    Add Test Parallelization (TestNG / Selenium Grid)
    File Upload/Import tests
    Connection String examples for databases
    Data Parsing
    Test data generators
    Add Appium support for mobile
    Load Testing examples (Likely JMeter)
    Add more test websites with varying levels of difficulty. Things with graphs, shadow-root elements, 
        and really janky iframes maybe...

Helpful Links================================================================================
https://dotnet.microsoft.com/en-us/download/dotnet/10.0
https://nunit.org/
https://www.seleniumhq.org/docs/
https://extentreports.com/docs/versions/5/net/index.html
https://devhints.io/xpath
https://github.com/appium/dotnet-client
https://www.automatetheplanet.com/most-complete-appium-csarp-cheat-sheet/
https://appium.io/docs/en/2.3/quickstart/test-dotnet/
https://abstracta.github.io/jmeter-dotnet-dsl/guide/
https://github.com/abstracta/jmeter-dotnet-dsl

