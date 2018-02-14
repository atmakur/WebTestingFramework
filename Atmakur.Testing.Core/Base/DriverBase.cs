using Atmakur.Testing.Core.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;

namespace Atmakur.Testing.Core
{
    public class DriverBase
    {
        private string _browserName;
        private string _browserVersion;
        private string _operatingSystem;

        public DriverBase(string browser, string version, string operatingSystem)
        {
            _browserName = browser.ToLower();
            _browserVersion = version;
            _operatingSystem = operatingSystem;
        }

        public IWebDriver GetDriver()
        {
            var driver = AppConfig.UseSauceLabs || AppConfig.UseSeleniumGrid ? GetRemoteDriver() : GetLocalDriver();
            driver.Manage().Window.Maximize();

            return driver;
        }

        private RemoteWebDriver GetRemoteDriver()
        {
            Uri _remoteDriverUri = AppConfig.UseSauceLabs ? new Uri(AppConfig.SauceRemoteDriverUri) : new Uri(AppConfig.SeleniumGridDriverUri);

            var driverCapabilities = new DesiredCapabilities();
            driverCapabilities.SetCapability(CapabilityType.BrowserName, _browserName);
            driverCapabilities.SetCapability(CapabilityType.BrowserVersion, _browserVersion);

            if (AppConfig.UseSauceLabs)
            {
                driverCapabilities.SetCapability("build", string.Format("{0}: {1}", AppConfig.BuildName, AppConfig.BuildId));
                driverCapabilities.SetCapability(CapabilityType.Platform, _operatingSystem);
                driverCapabilities.SetCapability("username", AppConfig.SL_UserName);
                driverCapabilities.SetCapability("accessKey", AppConfig.SL_AccessKey);
                driverCapabilities.SetCapability("parentTunnel", AppConfig.ParentTunnelId);
                driverCapabilities.SetCapability("tunnelIdentifier", AppConfig.TunnelId);
                driverCapabilities.SetCapability("name", TestContext.CurrentContext.Test.Name + "-" + _browserName);

                foreach (var capability in AppConfig.Capabilities)
                {
                    var splittedCap = capability.Split(':');
                    if (splittedCap.Length == 2)
                        driverCapabilities.SetCapability(splittedCap[0], splittedCap[1]);
                }
            }
            return new RemoteWebDriver(_remoteDriverUri, driverCapabilities, AppConfig.SeleniumCommandWaitTime);
        }

        private IWebDriver GetLocalDriver()
        {
            switch (_browserName)
            {
                case "chrome":
                    return new ChromeDriver();
                case "microsoftedge":
                    return new EdgeDriver();
                case "firefox":
                    return new FirefoxDriver();
                case "internet explorer":
                    return new InternetExplorerDriver();
                default:
                    throw new InvalidOperationException("Driver type not supported");
            }
        }
    }
}
