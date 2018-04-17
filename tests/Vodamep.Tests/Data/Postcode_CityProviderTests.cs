using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class Postcode_CityProviderTests
    {
        [Theory]
        [InlineData("6900 Bregenz", true)]
        [InlineData("Bregenz", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void IsValid(string code, bool expected)
        {
            var p = Postcode_CityProvider.Instance;

            Assert.Equal(expected, p.IsValid(code));
        }
    }
}
