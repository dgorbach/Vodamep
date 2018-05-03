using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{

    internal class ActivityValidator123And15 : AbstractValidator<HkpvReport>
    {
        public ActivityValidator123And15()
        {
            RuleFor(x => x.Activities)
                .Custom((list, ctx) =>
                {
                    if (!list?.Any() ?? false)
                        return;

                    var l = list
                        .Where(x => !string.IsNullOrEmpty(x.Date))
                        .Where(x => x.Amount > 0)
                        .Where(x => !string.IsNullOrEmpty(x.StaffId))
                        .Where(x => !string.IsNullOrEmpty(x.PersonId));

                    var entries123 = l.Where(x => x.Type == ActivityType.Lv01 || x.Type == ActivityType.Lv02 || x.Type == ActivityType.Lv03)
                        .GroupBy(x => new DateStaffPerson { Date = x.DateD, StaffId = x.StaffId, PersonId = x.PersonId })
                        .Select(x => x.Key)
                        .ToArray();

                    var entries4 = l.Where(x => ((int)x.Type > 3))
                        .GroupBy(x => new DateStaffPerson { Date = x.DateD, StaffId = x.StaffId, PersonId = x.PersonId })
                        .Select(x => x.Key)
                        .ToArray();

                    var error1 = entries123.Except(entries4).ToArray();
                    var error2 = entries4.Except(entries123).ToArray();

                    foreach (var entry in error1)
                    {
                        var item = list.Where(x => x.DateD == entry.Date && x.StaffId == entry.StaffId && x.PersonId == entry.PersonId).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", Validationmessages.WithoutEntry("15")));
                    }

                    foreach (var entry in error2)
                    {                        
                        var item = list.Where(x => x.DateD == entry.Date && x.StaffId == entry.StaffId && x.PersonId == entry.PersonId).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", Validationmessages.WithoutEntry("1,2,3")));

                    }
                });
        }        
    }
}
