using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class ActivityMedicalByStaffRoleTraineeValidator : AbstractValidator<HkpvReport>
    {

        public ActivityMedicalByStaffRoleTraineeValidator()
            : base()
        {
            this.RuleFor(x => new { x.Activities, x.Staffs })
                .Custom((a, ctx) =>
               {
                   var trainees = a.Staffs.Where(x => x.Role == StaffRole.Trainee).Select(x => x.Id).ToArray();

                   var medical = a.Activities
                       .Where(IsMedical)
                       .Where(x => trainees.Contains(x.StaffId))
                       .ToArray();

                   foreach ( var entry in medical)
                   {
                       var staff = a.Staffs.Where(x => x.Id == entry.StaffId).First();
                       
                       var msg = Validationmessages.TraineeMustNotContain06To10(staff);
                       
                       var index = a.Activities.IndexOf(entry);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", msg));
                   }
               });
        }

        internal bool IsMedical(Activity activity) => activity.Type == ActivityType.Lv06 || activity.Type == ActivityType.Lv07 || activity.Type == ActivityType.Lv08 || activity.Type == ActivityType.Lv09 || activity.Type == ActivityType.Lv10;

    }
}
