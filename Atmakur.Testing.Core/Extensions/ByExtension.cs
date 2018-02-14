using Atmakur.Testing.Core.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;

namespace Atmakur.Testing.Core.Extensions
{
    public static class ByExtensions
    {
        public static void Click(this By by, IWebDriver driver) => driver.FindElement(by).Click();
        public static string Text(this By by, IWebDriver driver) => driver.FindElement(by).Text;
        public static string Value(this By by, IWebDriver driver) => driver.FindElement(by).GetValueAttribute();
        public static IWebElement FindElement(this By by, IWebDriver driver) => driver.FindElement(by);
        public static ReadOnlyCollection<IWebElement> FindElements(this By by, IWebDriver driver) => driver.FindElements(by);
        public static void EnterText(this By by, string text, IWebDriver driver) => driver.FindElement(by).EnterText(text);
        public static bool IsEnabled(this By by, IWebDriver driver) => driver.FindElement(by).Enabled;
        public static bool IsVisible(this By by, IWebDriver driver) => by.IsEnabled(driver) && by.Displayed(driver);
        
        public static bool Displayed(this By by, IWebDriver driver)
        {
            try
            {
                return driver.FindElement(by).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public static bool Exists(this By by, IWebDriver driver)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static void WaitForElementDisplayed(this By by, IWebDriver driver, TimeSpan? timeout = null, bool suppressException = true)
        {
            var waitTimeout = timeout.IsNotNull() ? timeout.Value : AppConfig.ElementLoadWaitTime;
            try
            {
                var wait = new WebDriverWait(driver, waitTimeout);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(d => d.FindElement(by).Displayed && d.FindElement(by).Enabled);
                by.ScrollToElement(driver);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        public static void ScrollToElement(this By by, IWebDriver driver)
        {
            IWebElement element = driver.FindElement(by);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            System.Threading.Thread.Sleep(500);
        }

        public static void WaitForElementExists(this By by, IWebDriver driver, TimeSpan? timeout = null, bool suppressException = true)
        {
            var waitTimeout = timeout.IsNotNull() ? timeout.Value : AppConfig.ElementLoadWaitTime;
            try
            {
                var wait = new WebDriverWait(driver, waitTimeout);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(d => by.Exists(d));
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        public static void WaitForTextExists(this By by, IWebDriver driver, bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, AppConfig.ElementLoadWaitTime);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(d => by.Text(driver).Trim().Length > 0);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        public static void WaitForMinimumTextLength(this By by, IWebDriver driver, int minTextLength, bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, AppConfig.ElementLoadWaitTime);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(d => by.Text(driver).Trim().Length >= minTextLength);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }
    }
}
