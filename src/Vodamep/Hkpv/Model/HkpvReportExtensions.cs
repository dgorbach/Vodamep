using System;
using System.Collections.Generic;
using System.IO;

namespace Vodamep.Hkpv.Model
{

    public static class HkpvReportExtensions
    {

        public static HkpvReport AddPerson(this HkpvReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static HkpvReport AddPersons(this HkpvReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        public static HkpvReport AddPerson(this HkpvReport report, (Person Person, PersonalData Data) p)
        {
            if (p.Person != null)
                report.Persons.Add(p.Person);

            if (p.Data != null)
                report.PersonalData.Add(p.Data);

            return report;
        }

        public static HkpvReport AddPersons(this HkpvReport report, (Person Person, PersonalData Data)[] ps)
        {
            foreach (var p in ps)
            {
                if (p.Person != null)
                    report.Persons.Add(p.Person);

                if (p.Data != null)
                    report.PersonalData.Add(p.Data);
            }

            return report;
        }

        private static HkpvReport InvokeAndReturn(this HkpvReport m, Action<HkpvReport> action)
        {
            action(m);
            return m;
        }
        
        public static string WriteToFile(this HkpvReport report, bool asJson, string path = "", bool compressed = true) => new HkpvReportSerializer().WriteToFile(report, asJson, path, compressed);

    }
}