using FluentValidation;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class InstitutionValidator : AbstractValidator<Institution>
    {
        public InstitutionValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.Name).NotEmpty();
        }
    }
}
