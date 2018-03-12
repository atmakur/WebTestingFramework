using Atmakur.Testing.Core;
using Atmakur.Testing.Core.Extensions;
using Atmakur.Testing.Core.Utilities;
using Atmakur.Testing.Pages;
using Atmakur.Testing.Pages.NavigationPages;
using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace WebTestingFramework.Base
{
    [TestFixture]
    public class TestsBase : IWebTest
    {
        public IWebDriver BaseWebDriver { get; private set; }
        public TestCaseResult TestContextResult { get; set; }
        private DriverBase _driverBase;
        private DateTime _testStartTime;
        private TestContext _currentTestCtx = null;
        private TimeSpan _testDuration;

        public TestsBase(string browser, string version, string platform)
        {
            TestContextResult = new TestCaseResult();
            _testStartTime = DateTime.Now;
            _currentTestCtx = TestContext.CurrentContext;
            TestContextResult.Add("id", _currentTestCtx.Test.ID);
            TestContextResult.Add("TestStartTime", _testStartTime.ToFormattedString());
            TestContextResult.Add("TestName", _currentTestCtx.Test.FullName);
            TestContextResult.Add("Browser", browser);
            TestContextResult.Add("Version", version);
            TestContextResult.Add("Platform", platform);
            _driverBase = new DriverBase(browser, version, platform);
        }

        public BaseNavigationProvider Initialize()
        {
            BaseWebDriver = _driverBase.GetDriver();
            return new GooglePage(this);
        }

        ~TestsBase()
        {
            TestContextResult.Add("TestCaseResult", _currentTestCtx.Result.Outcome.Status.ToString());
            if (_currentTestCtx.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed)
                PassedTest();
            else
                FailedTest();
            DisposeWebDriverInstance();
            _testDuration = DateTime.Now - _testStartTime;
            TestContextResult.Add("TestEndTime", DateTime.Now.ToFormattedString());
            TestContextResult.Add("TestDuration", string.Format("{0} seconds", _testDuration.TotalSeconds));
            TestResultsHelper.GenerateOutputResults(TestContextResult);
        }

        private void DisposeWebDriverInstance()
        {
            if (BaseWebDriver.IsNotNull())
            {
                BaseWebDriver.Quit();
                BaseWebDriver.Dispose();
                BaseWebDriver = null;
            }
        }

        private void PassedTest()
        {
            if (AppConfig.UseSauceLabs && BaseWebDriver.IsNotNull())
            {
                ((IJavaScriptExecutor)BaseWebDriver).ExecuteScript("sauce:job-result=passed");
            }
        }

        private void FailedTest()
        {
            if (AppConfig.UseSauceLabs && BaseWebDriver.IsNotNull())
            {
                ((IJavaScriptExecutor)BaseWebDriver).ExecuteScript("sauce:job-result=failed");
            }
        }
    }
}
