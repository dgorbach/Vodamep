using FluentValidation;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class StaffValidator : AbstractValidator<Staff>
    {
        public StaffValidator()
        {
            this.RuleFor(x => x.FamilyName).NotEmpty();
            this.RuleFor(x => x.GivenName).NotEmpty();            
        }
    }
}
