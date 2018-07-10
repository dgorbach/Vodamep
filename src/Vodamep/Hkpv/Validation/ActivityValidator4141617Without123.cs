using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class ActivityValidator4141617Without123 : AbstractValidator<Activity>
    {
        public ActivityValidator4141617Without123()
        {
            RuleFor(x => x.Entries)
                .Custom((list, ctx) =>
                {
                    if (!list?.Any() ?? false)
                        return;

                    var l = list;

                    var entries123 = l.Where(x => x == ActivityType.Lv01 || x == ActivityType.Lv02 || x == ActivityType.Lv03).Any();
                    
                    var entries4Except15 = l.Where(x => x != ActivityType.Lv15 && ((int)x > 3) && ((int)x <= 17)).Any();
                                        
                    if (entries4Except15 && !entries123)
                    {
                        ctx.AddFailure(new ValidationFailure($"{nameof(Activity.Entries)}", Validationmessages.WithoutEntry("1,2,3")));
                    }
                });
        }
    }
}
