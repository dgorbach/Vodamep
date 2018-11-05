using FluentValidation;
using System.Text.RegularExpressions;
using Vodamep.Data;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class StaffValidator : AbstractValidator<Staff>
    {
        public StaffValidator()
        {
            this.RuleFor(x => x.FamilyName).NotEmpty();
            this.RuleFor(x => x.GivenName).NotEmpty();

            // Änderung 5.11.2018, LH
            var r = new Regex(@"^[\p{L}][-\p{L} ]*[\p{L}]$");
            this.RuleFor(x => x.FamilyName).Matches(r).Unless(x => string.IsNullOrEmpty(x.FamilyName));
            this.RuleFor(x => x.GivenName).Matches(r).Unless(x => string.IsNullOrEmpty(x.GivenName));
            
            this.RuleFor(x => x.Qualification).SetValidator(new CodeValidator<QualificationCodeProvider>()).Unless(x => string.IsNullOrEmpty(x.Qualification));
        }
    }
}
