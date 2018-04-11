using FluentValidation;
using Vodamep.Data;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.Insurance).NotEmpty();
            this.RuleFor(x => x.Insurance).SetValidator(new CodeValidator<InsuranceCodeProvider>());

            this.RuleFor(x => x.Religion).NotEmpty();
            this.RuleFor(x => x.Religion).SetValidator(new CodeValidator<ReligionCodeProvider>());

            this.RuleFor(x => x.Nationality).NotEmpty();
            this.RuleFor(x => x.Nationality).SetValidator(new CodeValidator<CountryCodeProvider>());

            this.RuleFor(x => x.CareAllowance).NotEqual(CareAllowance.UndefinedAllowance);

            this.RuleFor(x => x.Postcode).NotEmpty();
            this.RuleFor(x => x.City).NotEmpty();

            this.RuleFor(x => x.Gender).NotEqual(Gender.UndefinedGender);


        }
    }
}
