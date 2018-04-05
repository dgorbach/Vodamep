using FluentValidation;
using Vodamep.Data;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class PersonalDataValidator :    AbstractValidator<PersonalData>
    {
        public PersonalDataValidator()
        {
            this.RuleFor(x => x.FamilyName).NotEmpty();
            this.RuleFor(x => x.GivenName).NotEmpty();
            this.RuleFor(x => x.Street).NotEmpty();
            this.RuleFor(x => x.Postcode).NotEmpty();
            this.RuleFor(x => x.City).NotEmpty();

            this.RuleFor(x => x.Country).NotEmpty();            
            this.RuleFor(x => x.Country).SetValidator(new CodeValidator<CountryCodeProvider>());

            this.Include(new PersonalDataBirthdayValidator());
            this.Include(new PersonalDataSsnValidator());
        }
    }
}
