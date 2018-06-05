using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{

    internal class ActivityValidator23Without417 : AbstractValidator<HkpvReport>
    {
        public ActivityValidator23Without417()
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



                    var entries23 = l.Where(x => x.Type == ActivityType.Lv02 || x.Type == ActivityType.Lv03)
                        .GroupBy(x => new DateStaffPerson { Date = x.DateD, StaffId = x.StaffId, PersonId = x.PersonId })
                        .Select(x => x.Key)
                        .ToArray();

                    var entries4 = l.Where(x => ((int)x.Type > 3))
                        .GroupBy(x => new DateStaffPerson { Date = x.DateD, StaffId = x.StaffId, PersonId = x.PersonId })
                        .Select(x => x.Key)
                        .ToArray();



                    var errorWithout4 = entries23.Except(entries4).ToArray();


                    foreach (var entry in errorWithout4)
                    {
                        var item = list.Where(x => x.DateD == entry.Date && x.StaffId == entry.StaffId && x.PersonId == entry.PersonId).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", Validationmessages.WithoutEntry("4-17")));
                    }

                });
        }
    }
}
