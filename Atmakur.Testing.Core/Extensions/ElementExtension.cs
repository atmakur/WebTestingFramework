using OpenQA.Selenium;

namespace Atmakur.Testing.Core.Extensions
{
    public static class ElementExtension
    {
        public static string GetValueAttribute(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        public static void EnterText(this IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);
        }
    }
}
