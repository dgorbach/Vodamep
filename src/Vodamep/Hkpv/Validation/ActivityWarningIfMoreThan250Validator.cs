using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class ActivityWarningIfMoreThan250Validator : AbstractValidator<HkpvReport>
    {
        public ActivityWarningIfMoreThan250Validator()
            : base()
        {
            this.RuleFor(x => new Tuple<IList<Activity>, IEnumerable<Person>>(x.Activities, x.Persons))
                .Custom((a, ctx) =>
                {
                    var moreThan250 = a.Item1.Where(x => x.PersonId != string.Empty)
                        .GroupBy(x => x.PersonId)
                        .Select(x => new { PersonId = x.Key, Sum = x.Sum(y => y.GetLP() * y.Amount) })
                        .Where(x => x.Sum > 250);

                    foreach (var entry in moreThan250)
                    {
                        var p = a.Item2.Where(x => x.Id == entry.PersonId).First();

                        var f = new ValidationFailure($"{nameof(HkpvReport)}", Validationmessages.ActivityMoreThen250(p, entry.Sum));
                        f.Severity = Severity.Warning;

                        ctx.AddFailure(f);
                    }
                });
        }

    }
}
