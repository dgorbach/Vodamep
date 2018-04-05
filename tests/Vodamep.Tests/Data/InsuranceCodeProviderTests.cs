using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class InsuranceCodeProviderTests
    {
        [Theory]
        [InlineData("19", true)]
        [InlineData("GKK", false)]
        [InlineData(null, false)]
        [InlineData("", false)]        
        public void IsValid(string code, bool expected)
        {
            var p = InsuranceCodeProvider.Instance;

            Assert.Equal(expected, p.IsValid(code));
        }
    }
}
