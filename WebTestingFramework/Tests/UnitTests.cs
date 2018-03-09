using Atmakur.Testing.Core.Utilities;
using NUnit.Framework;
using WebTestingFramework.Base;
using System;

namespace WebTestingFramework.Tests
{
    [TestFixtureSource(typeof(TextFixtureArgs))]
    public class UnitTests1
    {
        string _browserName = string.Empty;
        string _version = string.Empty;
        string _platform = string.Empty;

        public UnitTests1(string browser, string version, string platform)
        {
            _browserName = browser;
            _version = version;
            _platform = platform;
        }

        [Test]
        public void MyTest1()
        {
            var testBase = new TestsBase(_browserName, _version, _platform);

            testBase.
                Initialize()
                .NavigateToHomePage()
                .EnterQueryText("Platinum")
                .ValidateSearchResult("Platinum - Google Search");
        }

        [Test]
        public void MyTest2()
        {
            var testBase = new TestsBase(_browserName, _version, _platform);
            testBase.
                Initialize()
                .NavigateToHomePage()
                .EnterQueryText("Gold")
                .ValidateSearchResult("Gold - Google Search");
        }

        [Test]
        public void MyTest3()
        {
            var testBase = new TestsBase(_browserName, _version, _platform);

            testBase.
                Initialize()
                .NavigateToHomePage()
                .EnterQueryText("Silver")
                .ValidateSearchResult("Silver - Google Search");
        }

        [Test]
        public void MyTest4()
        {
            var testBase = new TestsBase(_browserName, _version, _platform);

            testBase.
                Initialize()
                .NavigateToHomePage()
                .EnterQueryText("Bronze")
                .ValidateSearchResult("Bronze - Google Search");
        }
        
        [TearDown]
        public void Cleanup()
        {
            GC.Collect;
        }
    }
}
