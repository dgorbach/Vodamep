using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class PersonalDataSsnIsUniqueValidator : AbstractValidator<HkpvReport>
    {
        public PersonalDataSsnIsUniqueValidator()
        {
            RuleFor(x => x.PersonalData)
                .Custom((list, ctx) =>
                {
                    var dublicates = list.Where(x => !string.IsNullOrEmpty(x.Ssn))
                        .GroupBy(x => x.Ssn)
                        .Where(x => x.Count() > 1);

                    foreach (var entry in dublicates)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(HkpvReport.PersonalData), Validationmessages.SsnNotUnique(entry)));
                    }
                });
        }

    }
}
