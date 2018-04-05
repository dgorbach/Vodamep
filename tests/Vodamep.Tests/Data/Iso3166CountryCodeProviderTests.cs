using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class Iso3166CountryCodeProviderTests
    {
        [Theory]
        [InlineData("AT", true)]
        [InlineData("DE", true)]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("Österreich", false)]
        public void IsValid(string code, bool expected)
        {
            var p = CountryCodeProvider.Instance;

            Assert.Equal(expected, p.IsValid(code));
        }
    }
}
