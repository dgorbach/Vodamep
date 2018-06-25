using Google.Protobuf.WellKnownTypes;
using System;
using System.Globalization;

namespace Vodamep
{
    internal static class DateTimeExtensions
    {
        public static DateTime LastDateInMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);

        public static DateTime FirstDateInMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

        public static Timestamp AsTimestamp(this DateTime date) => Timestamp.FromDateTime(date.IgnoreTime());

        public static DateTime AsDate(this Timestamp value) => value?.ToDateTime().IgnoreTime() ?? DateTime.MinValue;

        public static DateTime IgnoreTime(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Wandelt einen string im format yyyy-MM-dd in ein Datum
        /// </summary>        
        public static DateTime AsDate(this string value) => DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture).IgnoreTime();


    }
}
