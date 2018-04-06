using FluentValidation;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Vodamep.Data.Dummy;
using Vodamep.Hkpv.Model;
using Xunit;

namespace Vodamep.Hkpv.Validation.Tests
{
    public abstract class ValidationTestsBase
    {
        public ValidationTestsBase()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            var loc = new DisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            this.Report = DataGenerator.Instance.CreateHkpvReport(0, 0, false);
        }


        protected HkpvReport Report { get; }


        protected void AssertError(string error)
        {
            var v = new HkpvReportValidator();
            var result = v.Validate(this.Report);

            Assert.Contains(error, result.Errors.Select(x => x.ErrorMessage));
        }

        protected void AssertErrorRegExp(string pattern)
        {
            var r = new Regex(pattern);

            var v = new HkpvReportValidator();
            var result = v.Validate(this.Report);

            Assert.True(result.Errors.Where(x => r.IsMatch(x.ErrorMessage)).Any());
        }
    }
}
