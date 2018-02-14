using Atmakur.Testing.Core.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

namespace Atmakur.Testing.Core.Extensions
{
    public static class DriverExtensions
    {
        public static void WaitForPageLoad(this IWebDriver driver)
        {
            var timer = new Stopwatch();
            timer.Start();
            while (timer.Elapsed < AppConfig.ElementLoadWaitTime)
            {
                var wait = new WebDriverWait(driver, AppConfig.ElementLoadWaitTime);
                try
                {
                    wait.Until(x => x.IsDocumentDone() && x.IsAjaxDone() && x.IsAngularDone());
                    break;
                }
                catch (InvalidOperationException)
                {

                }
            }
        }
        
        public static void CheckForNetworkLogin(this IWebDriver driver, long timeSpanTicks)
        {
            try
            {
                Thread.Sleep(15000);
                if (timeSpanTicks <= 0) timeSpanTicks = TimeSpan.TicksPerMinute;
                var wait = new WebDriverWait(driver, new TimeSpan(timeSpanTicks));
                wait.IgnoreExceptionTypes(typeof(UnhandledAlertException));
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(ExpectedConditions.AlertIsPresent());
                var alert = driver.SwitchTo().Alert();
                alert.SetAuthenticationCredentials(Environment.GetEnvironmentVariable("NTUserName"), Environment.GetEnvironmentVariable("NTPassword"));
                alert.Accept();
            }
            catch (Exception ex)
            {
                Console.WriteLine("WaitTime: {2}, Exception: {0}, StackTrace: {1}", ex.Message, ex.StackTrace, new TimeSpan(timeSpanTicks).Seconds);
            }
        }

        private static bool IsAngularDone(this IWebDriver webDriver)
        {
            try
            {
                var script = @"if(window.angular === undefined) return true;
                           if(!angular.element($('.ng-scope')).injector()) angular.reloadWithDebugInfo(); 
                           return angular.element($('.ng-scope')).injector().get('$http').pendingRequests.length == 0;";
                return webDriver.ExecuteJavaScript<bool>(script);
            }
            catch
            {
                var script = @"return (window.angular !== undefined) && (angular.element(document).injector() !== undefined) && (angular.element(document).injector().get('$http').pendingRequests.length === 0)";
                return webDriver.ExecuteJavaScript<bool>(script);
            }
        }

        private static bool IsAjaxDone(this IWebDriver webDriver)
        {
            var script = "return window.jQuery === undefined || jQuery.active == 0";
            var isAjaxDone = webDriver.ExecuteJavaScript<bool>(script);
            return isAjaxDone;
        }

        private static bool IsDocumentDone(this IWebDriver webDriver)
        {
            var script = "return document.readyState == 'complete'";
            var isAjaxDone = webDriver.ExecuteJavaScript<bool>(script);
            return isAjaxDone;
        }

        public static IWebElement WaitForAtleastOneElement(this IWebDriver driver, By by1, By by2)
        {
            TimeSpan timeout = AppConfig.ElementLoadWaitTime;
            var timer = new Stopwatch();
            IWebElement webElement = null;
            while (timer.Elapsed < timeout)
            {
                try
                {
                    webElement = driver.FindElement(by1);
                    if (webElement.Displayed)
                        break;
                }
                catch { }
                try
                {
                    webElement = driver.FindElement(by2);
                    if (webElement.Displayed)
                        break;
                }
                catch { }
            }
            return webElement;
        }

        public static void BringIntoView(this IWebDriver driver, By by)
        {
            var element = by.FindElement(driver);
            var jsExec = driver.InjectJQuery();
            ICoordinates coordinates = ((ILocatable)element).Coordinates;
            Point point = coordinates.LocationInViewport;
            if (driver.IsClickObstructed(point, element))
            {
                jsExec.ExecuteScript("function scrollIntoView(el) {"
                    + "var offsetTop = $(el).offset().top;"
                    + "var adjustment = Math.max(0,( $(window).height() - $(el).outerHeight(true) ) / 2);"
                    + "var scrollTop = offsetTop - adjustment;"
                    + "$('html,body').animate({"
                    + "scrollTop: scrollTop"
                    + "}, 0);"
                    + "} scrollIntoView(arguments[0]);", element);
            }
        }

        private static bool IsJQueryLoaded(this IWebDriver webDriver)
        {
            var script = "return jQuery() != null";
            var isAjaxDone = webDriver.ExecuteJavaScript<bool>(script);
            return isAjaxDone;
        }

        private static bool IsClickObstructed(this IWebDriver driver, Point point, IWebElement element)
        {
            Size dim = element.Size;
            int clickX = point.X + (dim.Width / 2);
            int clickY = point.Y + (dim.Height / 2);
            // now get web element at click location
            IWebElement elementAtClick = driver.ExecuteJavaScript<IWebElement>("return document.elementFromPoint(" + clickX + ", " + clickY + ");");
            string elementAtClickXPath = driver.GetAbsoluteXPath(elementAtClick);
            string elementXPath = driver.GetAbsoluteXPath(element);
            if (!elementXPath.Equals(elementAtClickXPath))
            {
                return true;
            }
            return false;
        }

        private static IJavaScriptExecutor InjectJQuery(this IWebDriver driver)
        {
            var JQUERY_URL = "http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js";
            IJavaScriptExecutor jsExec = (IJavaScriptExecutor)driver;
            if (driver.IsAjaxDone())
            {
                WebDriverWait wait = (new WebDriverWait(driver, AppConfig.ElementLoadWaitTime));
                jsExec.ExecuteScript("var headID = document.getElementsByTagName('head')[0];" +
                    "var newScript = document.createElement('script');" +
                    "newScript.type = 'text/javascript';" +
                    "newScript.src = '" + JQUERY_URL + "';" +
                    "headID.appendChild(newScript);");
                wait.Until(x => x.IsJQueryLoaded());
            }
            return jsExec;
        }

        private static string GetAbsoluteXPath(this IWebDriver driver, IWebElement element)
        {
            return driver.ExecuteJavaScript<string>("function absoluteXPath(element) {"
                    + "var comp, comps = [];"
                    + "var parent = null;"
                    + "var xpath = '';"
                    + "var getPos = function(element) {"
                    + "var position = 1, curNode;"
                    + "if (element.nodeType == Node.ATTRIBUTE_NODE) {"
                    + "return null;"
                    + "}"
                    + "for (curNode = element.previousSibling; curNode; curNode = curNode.previousSibling) {"
                    + "if (curNode.nodeName == element.nodeName) {"
                    + "++position;"
                    + "}"
                    + "}"
                    + "return position;"
                    + "};"
                    + "if (element instanceof Document) {"
                    + "return '/';"
                    + "}"
                    + "for (; element && !(element instanceof Document); element = element.nodeType == Node.ATTRIBUTE_NODE ? element.ownerElement : element.parentNode) {"
                    + "comp = comps[comps.length] = {};"
                    + "switch (element.nodeType) {"
                    + "case Node.TEXT_NODE:"
                    + "comp.name = 'text()';"
                    + "break;"
                    + "case Node.ATTRIBUTE_NODE:"
                    + "comp.name = '@' + element.nodeName;"
                    + "break;"
                    + "case Node.PROCESSING_INSTRUCTION_NODE:"
                    + "comp.name = 'processing-instruction()';"
                    + "break;"
                    + "case Node.COMMENT_NODE:"
                    + "comp.name = 'comment()';"
                    + "break;"
                    + "case Node.ELEMENT_NODE:"
                    + "comp.name = element.nodeName;"
                    + "break;"
                    + "}"
                    + "comp.position = getPos(element);"
                    + "}"
                    + "for (var i = comps.length - 1; i >= 0; i--) {"
                    + "comp = comps[i];"
                    + "xpath += '/' + comp.name.toLowerCase();"
                    + "if (comp.position !== null) {"
                    + "xpath += '[' + comp.position + ']';"
                    + "}"
                    + "}"
                    + "return xpath;"
                    + "} return absoluteXPath(arguments[0]);", element);
        }
    }
}
