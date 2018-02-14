using Atmakur.Testing.Core.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atmakur.Testing.Pages.NavigationPages
{
    public class GooglePage : BaseNavigationProvider
    {
        private By QueryTextBox = By.CssSelector("[name='q']");
        private By ResultPageDiv = By.CssSelector("#rcnt");
        public GooglePage(IWebTest webTest) : base(webTest)
        {

        }

        public GooglePage EnterQueryText(string queryText)
        {
            QueryTextBox.EnterText(queryText + Keys.Enter, WebDriver);
            return this;
        }

        public GooglePage ValidateSearchResult(string resultStr)
        {
            ResultPageDiv.WaitForElementDisplayed(WebDriver);
            Assert.AreEqual(resultStr, WebDriver.Title);
            Test.TestContextResult.Add("Result", WebDriver.Title);
            return this;
        }
    }
}
