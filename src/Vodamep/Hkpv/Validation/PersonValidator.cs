using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;
using Vodamep.Data;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.FamilyName).NotEmpty();
            this.RuleFor(x => x.GivenName).NotEmpty();

            // Wunsch von Gerhard
            // aus unterlagen_connexia: Daten.xsl
            var r = new Regex("^[a-zA-ZäöüÄÖÜß][-,.a-zA-ZäöüÄÖÜß ]*[,.a-zA-ZäöüÄÖÜß]$");
            this.RuleFor(x => x.FamilyName).Matches(r).Unless(x => string.IsNullOrEmpty(x.FamilyName));
            this.RuleFor(x => x.GivenName).Matches(r).Unless(x => string.IsNullOrEmpty(x.GivenName));

            this.Include(new PersonBirthdayValidator());
            this.Include(new PersonSsnValidator());

            this.RuleFor(x => x.Insurance).NotEmpty();
            this.RuleFor(x => x.Insurance).SetValidator(new CodeValidator<InsuranceCodeProvider>());

            this.RuleFor(x => x.Religion).NotEmpty();
            this.RuleFor(x => x.Religion).SetValidator(new CodeValidator<ReligionCodeProvider>());

            this.RuleFor(x => x.Nationality).NotEmpty();
            this.RuleFor(x => x.Nationality).SetValidator(new CodeValidator<CountryCodeProvider>());

            this.RuleFor(x => x.CareAllowance).NotEmpty();

            this.RuleFor(x => x.Postcode).NotEmpty();
            this.RuleFor(x => x.City).NotEmpty();
            this.RuleFor(x => $"{x.Postcode} {x.City}")
                .SetValidator(new CodeValidator<Postcode_CityProvider>())
                .Unless(x => string.IsNullOrEmpty(x.City) || string.IsNullOrEmpty(x.Postcode))
                .WithMessage(Validationmessages.InvalidPostCode_City);

            this.RuleFor(x => x.Gender).NotEmpty();


        }
    }
}
