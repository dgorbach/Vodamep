using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class HkpvReportStaffIdValidator : AbstractValidator<HkpvReport>
    {
        public HkpvReportStaffIdValidator()
        {
            this.RuleFor(x => x.Staffs)
                .Custom((list, ctx) =>
                {
                    foreach (var id in list.Select(x => x.Id).OrderBy(x => x).GroupBy(x => x).Where(x => x.Count() > 1))
                    {
                        var item = list.Where(x => x.Id == id.Key).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Staffs)}[{index}]", Validationmessages.IdIsNotUnique));
                    }
                });

            this.RuleFor(x => new { x.Staffs, x.Activities, x.OtherActivities, x.Consultations })
               .Custom((a, ctx) =>
               {
                   var idStaffs = a.Staffs.Select(x => x.Id).Distinct().ToArray();
                   var idActivities = (
                                a.Activities.Select(x => x.StaffId)
                                .Union(a.OtherActivities.Select(x => x.StaffId))
                                .Union(a.Consultations.Select(x => x.StaffId))
                                ).Distinct().ToArray();

                   foreach (var id in idStaffs.Except(idActivities))
                   {
                       var item = a.Staffs.Where(x => x.Id == id).First();
                       var index = a.Staffs.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Staffs)}[{index}]", Validationmessages.WithoutActivity));

                   }

                   foreach (var id in idActivities.Except(idStaffs))
                   {
                       ctx.AddFailure(new ValidationFailure(nameof(HkpvReport.Staffs), Validationmessages.IdIsMissing(id)));
                   }
               });

        }
    }
}
