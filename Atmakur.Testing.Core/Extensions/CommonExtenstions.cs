using System;

namespace Atmakur.Testing.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj) => obj == null;
        public static bool IsNotNull(this object obj) => obj != null;
    }

    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str, bool ignoreWhiteSpace = true) => ignoreWhiteSpace ? string.IsNullOrWhiteSpace(str) : string.IsNullOrEmpty(str);
        public static bool IsNotNullOrEmpty(this string str, bool ignoreWhiteSpace = true) => ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(str) : !string.IsNullOrEmpty(str);
        public static bool ToBool(this string str) => bool.TryParse(str, out bool result) ? result : false;
        public static int ToInt(this string str) => int.TryParse(str, out int result) ? result : 0;
        public static double ToDouble(this string str) => double.TryParse(str, out double result) ? result : 0;
    }

    public static class DateTimeExtenstion
    {
        public static string ToFormattedString(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        public static string ToNameString(this DateTime dateTime) => dateTime.ToString("yyyy.MM.dd.HH.mm.ss");
    }
}
