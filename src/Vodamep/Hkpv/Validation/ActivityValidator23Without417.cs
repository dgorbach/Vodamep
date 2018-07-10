using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{

    internal class ActivityValidator23Without417 : AbstractValidator<Activity>
    {
        public ActivityValidator23Without417()
        {
            RuleFor(x => x.Entries)
                .Custom((list, ctx) =>
                {
                    if (!list?.Any() ?? false)
                        return;

                    var l = list;



                    var entries23 = l.Where(x => x == ActivityType.Lv02 || x == ActivityType.Lv03).Any();


                    var entries4 = l.Where(x => ((int)x > 3)).Any();



                    if (entries23 && !entries4)
                    {
                        ctx.AddFailure(new ValidationFailure($"{nameof(Activity.Entries)}", Validationmessages.WithoutEntry("4-17")));
                    }


                });
        }
    }
}
