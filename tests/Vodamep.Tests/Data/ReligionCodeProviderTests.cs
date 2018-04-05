using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class ReligionCodeProviderTests
    {
        [Theory]
        [InlineData("VAR", true)]
        [InlineData("r.k.", false)]
        [InlineData(null, false)]
        [InlineData("", false)]        
        public void IsValid(string code, bool expected)
        {
            var p = ReligionCodeProvider.Instance;

            Assert.Equal(expected, p.IsValid(code));
        }
    }
}
