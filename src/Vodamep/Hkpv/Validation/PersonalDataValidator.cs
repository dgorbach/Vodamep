using FluentValidation;
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
            

            this.Include(new PersonalDataBirthdayValidator());
            this.Include(new PersonalDataSsnValidator());
        }
    }
}
