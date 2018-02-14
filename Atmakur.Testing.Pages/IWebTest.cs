using Atmakur.Testing.Core.Utilities;
using OpenQA.Selenium;

namespace Atmakur.Testing.Pages
{
    public interface IWebTest
    {
        IWebDriver BaseWebDriver { get; }
        TestCaseResult TestContextResult { get; }
    }
}
