using Atmakur.Testing.Core.Utilities;
using NUnit.Framework;
using WebTestingFramework.Base;
using System;

namespace WebTestingFramework.Tests
{
    [TestFixtureSource(typeof(TextFixtureArgs))]
    public class UnitTests2
    {
        string _browserName = string.Empty;
        string _version = string.Empty;
        string _platform = string.Empty;

        public UnitTests2(string browser, string version, string platform)
        {
            _browserName = browser;
            _version = version;
            _platform = platform;
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void MyTest5()
        {
            var testBase = new TestsBase(_browserName, _version, _platform);

            testBase.
                Initialize()
                .NavigateToHomePage()
                .EnterQueryText("Iron")
                .ValidateSearchResult("Iron - Google Search");
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void MyTest6()
        {
            var testBase = new TestsBase(_browserName, _version, _platform);
            testBase.
                Initialize()
                .NavigateToHomePage()
                .EnterQueryText("Steel")
                .ValidateSearchResult("Steel - Google Search");
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void MyTest7()
        {
            var testBase = new TestsBase(_browserName, _version, _platform);

            testBase.
                Initialize()
                .NavigateToHomePage()
                .EnterQueryText("Aluminium")
                .ValidateSearchResult("Aluminium - Google Search");
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void MyTest8()
        {
            var testBase = new TestsBase(_browserName, _version, _platform);

            testBase.
                Initialize()
                .NavigateToHomePage()
                .EnterQueryText("Gunmetal")
                .ValidateSearchResult("Gunmetal - Google Search");
        }

        [TearDown]
        public void Cleanup()
        {
            GC.Collect();
        }
    }
}
