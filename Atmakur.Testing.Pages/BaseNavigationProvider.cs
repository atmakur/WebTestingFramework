using Atmakur.Testing.Core.Extensions;
using Atmakur.Testing.Core.Utilities;
using Atmakur.Testing.Pages.NavigationPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Atmakur.Testing.Pages
{
    public abstract class BaseNavigationProvider
    {
        protected IWebTest Test;
        public IWebDriver WebDriver;

        public BaseNavigationProvider(IWebTest webTest)
        {
            Test = webTest;
            WebDriver = Test.BaseWebDriver;
            PageFactory.InitElements(WebDriver, this);
        }

        public GooglePage NavigateToHomePage()
        {
            WebDriver.Navigate().GoToUrl(AppConfig.InitUrl);
            WebDriver.WaitForPageLoad();
            return new GooglePage(Test);
        }
    }
}
