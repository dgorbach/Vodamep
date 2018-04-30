using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class ActivityWarningIfMoreThan5Validator : AbstractValidator<HkpvReport>
    {
        public ActivityWarningIfMoreThan5Validator()
            : base()
        {
            RuleFor(x => x.Activities)
                .Custom((list, ctx) =>
                {
                    var moreThan5 = list.Where(x => x.Amount > 5);

                    foreach (var entry in moreThan5)
                    {
                        var index = list.IndexOf(entry);

                        var f = new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", Validationmessages.ActivityMoreThenFive);
                        f.Severity = Severity.Warning;

                        ctx.AddFailure(f);
                    }
                });
        }

    }
}
