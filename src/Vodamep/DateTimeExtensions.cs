using System;
using System.Globalization;

namespace Vodamep
{
    internal static class DateTimeExtensions
    {
        public static DateTime LastDateInMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);

        public static DateTime FirstDateInMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

        public static string AsValue(this DateTime date) => date.ToString("yyyy-MM-dd");

        public static DateTime AsDate(this string value) => DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : DateTime.MinValue;

        
    }
}
