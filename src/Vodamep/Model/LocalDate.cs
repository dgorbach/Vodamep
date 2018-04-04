using Google.Protobuf;
using System;
using System.Globalization;
using System.Linq;

namespace Vodamep.Model
{
    public partial class LocalDate : IComparable, IComparable<LocalDate>, ICustomDiagnosticMessage
    {
        public static LocalDate Empty = new LocalDate();
        public static LocalDate MinDate = new LocalDate(DateTime.MinValue);
        public static LocalDate MaxDate = new LocalDate(DateTime.MaxValue);
        public static LocalDate Today => new LocalDate(DateTime.Today);
        public static LocalDate Now() => new LocalDate(DateTime.Now);

        public LocalDate AddDays(double value) => new LocalDate(this.ToDateTime().AddDays(value));

        public LocalDate AddMonths(int value) => new LocalDate(this.ToDateTime().AddMonths(value));

        public LocalDate AddYears(int value) => new LocalDate(this.ToDateTime().AddYears(value));


        public LocalDate(int year, int month, int day)
        {
            this.SetValue(year, month, day);
        }
        public LocalDate(DateTime date)
        {
            this.SetValue(date.Year, date.Month, date.Day);
        }

        partial void OnConstruction()
        {
            this.Value = "1900-01-01";
        }

        public int Year => int.TryParse(this.Value.Substring(0, 4), out int value) ? value : -1;
        public int Month => int.TryParse(this.Value.Substring(5, 2), out int value) ? value : -1;
        public int Day => int.TryParse(this.Value.Substring(8, 2), out int value) ? value : -1;

        public DayOfWeek DayOfWeek => this.ToDateTime().DayOfWeek;

        public LocalDate FirstDateInMonth() => new LocalDate(this.Year, this.Month, 1);
        public LocalDate LastDateInMonth() => new LocalDate(this.Year, this.Month, 1).AddMonths(1).AddDays(-1);

        private void SetValue(int year, int month, int day)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException($"Month {month} is not valid!");

            if (day < 1 || day > 31)
                throw new ArgumentOutOfRangeException($"Day {day} is not valid!");


            this.Value = $"{year:0000}-{month:00}-{day:00}";
        }

        public int CompareTo(LocalDate other) => this.Value.CompareTo(other?.Value ?? "");


        public DateTime ToDateTime() => new DateTime(this.Year, this.Month, this.Day);

        public int CompareTo(object obj) => this.CompareTo(obj as LocalDate);

        public string ToDiagnosticString() => this.Value;


        public static bool operator ==(LocalDate left, LocalDate right) => Equals(left, right);


        public static bool operator !=(LocalDate left, LocalDate right) => !Equals(left, right);


        public static bool operator >(LocalDate left, LocalDate right)
        {
            return left != null && right != null && (left.Year > right.Year || left.Month > right.Month || left.Day > right.Day);
        }

        public static bool operator <(LocalDate left, LocalDate right)
        {
            return left != null && right != null && (left.Year < right.Year || left.Month < right.Month || left.Day < right.Day);
        }

        public static bool operator >=(LocalDate left, LocalDate right)
        {
            return left == right || left > right;
        }

        public static bool operator <=(LocalDate left, LocalDate right)
        {
            return left == right || left < right;
        }

        public static LocalDate Min(params LocalDate[] dates)
        {
            var min = MaxDate;

            foreach (var date in dates.Where(x => x != null))
            {
                if (date < min)
                    min = date;
            }

            return min;
        }

        public static LocalDate Max(params LocalDate[] dates)
        {
            var max = MinDate;

            foreach (var date in dates.Where(x => x != null))
            {
                if (date > max)
                    max = date;
            }

            return max;
        }

        public string ToString(string format)
        {
            return this.ToDateTime().ToString(format);
        }


        public bool IsValid() => DateTime.TryParseExact(this.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

    }
}
