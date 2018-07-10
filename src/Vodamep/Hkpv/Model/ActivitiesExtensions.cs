using System.Collections.Generic;
using System.Linq;

namespace Vodamep.Hkpv.Model
{
    public static class ActivitiesExtensions
    {
        public static IEnumerable<Activity> AsSorted(this IEnumerable<Activity> activities)
        {
            var entries = new List<Activity>();
            // zuerst die Reihenfolge der Activity.Entries herstellen
            foreach (var a in activities)
            {
                var e = new Activity() { Date = a.Date, PersonId = a.PersonId, StaffId = a.StaffId };
                e.Entries.AddRange(a.Entries.OrderBy(x => x));

                entries.Add(e);
            }

            // jetzt die Einträge selbst sortieren
            return entries.OrderBy(x => x);
        }

        public static bool RequiresPersonId(this Activity activity) => activity.Entries.Where(x => x.RequiresPersonId()).Any();

        public static bool WithoutPersonId(this Activity activity) => activity.Entries.Where(x => x.WithoutPersonId()).Any();



    }
}