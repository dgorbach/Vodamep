using System;
using System.Globalization;
using Xunit;

namespace Vodamep.Model.Tests
{
    public class LocalDateTests
    {
        [Fact]
        public void ToString_ReturnsFormattedText()
        {
            var date = new LocalDate(2017, 06, 22);

            Assert.Equal("22.06.2017", date.ToString("dd.MM.yyyy"));
        }

        [Theory]
        [InlineData("2018.04.03", "2018.04.03", 0)]
        [InlineData("2018.04.03", "2018.04.04", -1)]
        [InlineData("2018.04.04", "2018.04.03", 1)]
        public void CompareTo(string d1, string d2, int expectedDir)
        {
            var date1 = DateTime.ParseExact(d1, "yyyy.MM.dd", CultureInfo.InvariantCulture);
            var date2 = DateTime.ParseExact(d2, "yyyy.MM.dd", CultureInfo.InvariantCulture);
            var ldate1 = new LocalDate(date1);
            var ldate2 = new LocalDate(date2);

            var c = date1.CompareTo(date2);

            var direction = c == 0 ? 0 : c < 0 ? -1 : 1;

            Assert.Equal(expectedDir, direction);
            Assert.Equal(date1.CompareTo(date2), ldate1.CompareTo(ldate2));
        }

        [Fact]
        public void Operators()
        {
            var date1 = new LocalDate(DateTime.Today);
            var date2 = new LocalDate(DateTime.Today.AddDays(1));


            Assert.False(date1 > date1.Clone());
            Assert.False(date1 < date1.Clone());

            Assert.True(date2 > date1);
            Assert.True(date1 < date2);


            Assert.True(date2 >= date1);
            Assert.True(date1 <= date2);

            Assert.True(date1 != date2);

            Assert.False(date1 == date2);
        }
    }
}
