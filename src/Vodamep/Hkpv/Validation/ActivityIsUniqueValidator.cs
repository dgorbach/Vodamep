using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{

    internal class ActivityIsUniqueValidator : AbstractValidator<HkpvReport>
    {
        public ActivityIsUniqueValidator()
            : base()
        {
            RuleFor(x => x.Activities)
                .Custom((list, ctx) =>
                {
                    var notUniques = list
                        .GroupBy(y => new { y.DateD, y.Type, y.PersonId, y.StaffId })
                        .Where(y => y.Count() > 1);
                        

                    foreach (var entry in notUniques)
                    {
                        var index = list.IndexOf(entry.First());
                        ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", Validationmessages.ActivityIsNotUnique));
                    }
                });
        }

    }
}
