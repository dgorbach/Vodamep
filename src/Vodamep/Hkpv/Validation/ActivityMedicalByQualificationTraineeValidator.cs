using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Data;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class ActivityMedicalByQualificationTraineeValidator : AbstractValidator<HkpvReport>
    {

        public ActivityMedicalByQualificationTraineeValidator()
            : base()
        {
            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später:  new  { x.Activities, x.Staffs }
            this.RuleFor(x => new Tuple<IList<Activity>, IEnumerable<Staff>>(x.Activities, x.Staffs))
                .Custom((a, ctx) =>
               {
                   var activities = a.Item1;
                   var staffs = a.Item2;

                   var trainees = staffs.Where(x => x.Qualification == QualificationCodeProvider.Instance.Trainee).Select(x => x.Id).ToArray();

                   var medical = activities
                       .Where(IsMedical)
                       .Where(x => trainees.Contains(x.StaffId))
                       .ToArray();

                   foreach ( var entry in medical)
                   {
                       var staff = staffs.Where(x => x.Id == entry.StaffId).First();
                       
                       var msg = Validationmessages.TraineeMustNotContain06To10(staff);
                       
                       var index = activities.IndexOf(entry);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", msg));
                   }
               });
        }

        internal bool IsMedical(Activity activity) => activity.Type == ActivityType.Lv06 || activity.Type == ActivityType.Lv07 || activity.Type == ActivityType.Lv08 || activity.Type == ActivityType.Lv09 || activity.Type == ActivityType.Lv10;

    }
}
