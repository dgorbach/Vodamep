using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{

    public class HkpvReportPersonIdValidator : AbstractValidator<HkpvReport>
    {
        public HkpvReportPersonIdValidator()
        {
            this.RuleFor(x => x.Persons)
                .Custom((list, ctx) =>
                {
                    foreach (var id in list.Select(x => x.Id).OrderBy(x => x).GroupBy(x => x).Where(x => x.Count() > 1))
                    {
                        var item = list.Where(x => x.Id == id.Key).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Persons)}[{index}]", Validationmessages.IdIsNotUnique));
                    }
                });

            this.RuleFor(x => x.PersonalData)
               .Custom((list, ctx) =>
               {
                   foreach (var id in list.Select(x => x.Id).OrderBy(x => x).GroupBy(x => x).Where(x => x.Count() > 1))
                   {
                       var item = list.Where(x => x.Id == id.Key).First();
                       var index = list.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.PersonalData)}[{index}]", Validationmessages.IdIsNotUnique));
                   }
               });

            this.RuleFor(x => new { x.Persons, x.PersonalData })
               .Custom((a, ctx) =>
               {
                   var idPersons = a.Persons.Select(x => x.Id).Distinct().ToArray();
                   var idPersonalData = a.PersonalData.Select(x => x.Id).Distinct().ToArray();

                   foreach (var id in idPersons.Except(idPersonalData))
                   {
                       var item = a.Persons.Where(x => x.Id == id).First();
                       var index = a.Persons.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Persons)}[{index}]", Validationmessages.PersonWithoutPersonalDate));
                   }

                   foreach (var id in idPersonalData.Except(idPersons))
                   {
                       var item = a.PersonalData.Where(x => x.Id == id).First();
                       var index = a.PersonalData.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.PersonalData)}[{index}]", Validationmessages.PersonWithoutData));
                   }
               });

            this.RuleFor(x => new { x.Persons, x.Activities })
               .Custom((a, ctx) =>
               {
                   var idPersons = a.Persons.Select(x => x.Id).Distinct().ToArray();
                   var idActivities = a.Activities.Select(x => x.PersonId).Distinct().ToArray();

                   foreach (var id in idPersons.Except(idActivities))
                   {
                       var item = a.Persons.Where(x => x.Id == id).First();
                       var index = a.Persons.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Persons)}[{index}]", Validationmessages.WithoutActivity));
                   }

                   foreach (var id in idActivities.Except(idPersons))
                   {
                       var item = a.Activities.Where(x => x.PersonId == id).First();
                       var index = a.Activities.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", Validationmessages.IdIsMissing(id)));
                   }
               });

        }
    }
}
