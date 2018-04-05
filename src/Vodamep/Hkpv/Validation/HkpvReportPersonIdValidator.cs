using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
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

            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später: new { x.Persons, x.PersonalData }
            this.RuleFor(x => new Tuple<IList<Person>, IList<PersonalData>>(x.Persons, x.PersonalData))
               .Custom((a, ctx) =>
               {
                   var persons = a.Item1;
                   var personalData = a.Item2;

                   var idPersons = persons.Select(x => x.Id).Distinct().ToArray();
                   var idPersonalData = personalData.Select(x => x.Id).Distinct().ToArray();

                   foreach (var id in idPersons.Except(idPersonalData))
                   {
                       var item = persons.Where(x => x.Id == id).First();
                       var index = persons.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Persons)}[{index}]", Validationmessages.PersonWithoutPersonalDate));
                   }

                   foreach (var id in idPersonalData.Except(idPersons))
                   {
                       var item = personalData.Where(x => x.Id == id).First();
                       var index = personalData.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.PersonalData)}[{index}]", Validationmessages.PersonWithoutData));
                   }
               });

            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später: new { x.Persons, x.Activities }
            this.RuleFor(x => new Tuple<IList<Person>, IList<Activity>>(x.Persons, x.Activities))
               .Custom((a, ctx) =>
               {
                   var persons = a.Item1;
                   var activities = a.Item2;

                   var idPersons = persons.Select(x => x.Id).Distinct().ToArray();
                   var idActivities = activities.Select(x => x.PersonId).Distinct().ToArray();

                   foreach (var id in idPersons.Except(idActivities))
                   {
                       var item = persons.Where(x => x.Id == id).First();
                       var index = persons.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Persons)}[{index}]", Validationmessages.WithoutActivity));
                   }

                   foreach (var id in idActivities.Except(idPersons))
                   {
                       var item = activities.Where(x => x.PersonId == id).First();
                       var index = activities.IndexOf(item);
                       ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", Validationmessages.IdIsMissing(id)));
                   }
               });

        }
    }
}
