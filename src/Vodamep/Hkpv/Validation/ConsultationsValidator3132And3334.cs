using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Google.Protobuf.Collections;
using System;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class ConsultationsValidator3132And3334 : AbstractValidator<HkpvReport>
    {

        public ConsultationsValidator3132And3334()
        {
            RuleFor(x => x.Consultations)
                .Custom(GetCutomValidation(ConsultationType.Lv31, ConsultationType.Lv32));

            RuleFor(x => x.Consultations)
                .Custom(GetCutomValidation(ConsultationType.Lv33, ConsultationType.Lv34));
        }

        private Action<RepeatedField<Consultation>, CustomContext> GetCutomValidation(ConsultationType type1, ConsultationType type2)
        {
            return (list, ctx) =>
            {
                if (!list?.Any() ?? false)
                    return;

                var entries31 = list.Where(x => x.Type == type1)
                    .GroupBy(x => new DateStaffPerson { Date = x.Date, StaffId = x.StaffId })
                    .Select(x => x.Key)
                    .ToArray();

                var entries32 = list.Where(x => x.Type == type2)
                    .GroupBy(x => new DateStaffPerson { Date = x.Date, StaffId = x.StaffId })
                    .Select(x => x.Key)
                    .ToArray();

                var error1 = entries31.Except(entries32).ToArray();
                var error2 = entries32.Except(entries31).ToArray();

                foreach (var entry in error1)
                {                    
                    var item = list.Where(x => x.Date == entry.Date && x.StaffId == entry.StaffId ).First();
                    var index = list.IndexOf(item);
                    ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Consultations)}[{index}]", Validationmessages.WithoutEntry(type2.ToString())));
                }

                foreach (var entry in error2)
                {
                    var item = list.Where(x => x.Date == entry.Date && x.StaffId == entry.StaffId).First();
                    var index = list.IndexOf(item);
                    ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Consultations)}[{index}]", Validationmessages.WithoutEntry(type1.ToString())));
                }
            };
        }
    }
}
