using System.Collections.Generic;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class DisplayNameResolver
    {
        private readonly IDictionary<string, string> _dict = new SortedDictionary<string, string>();

        public DisplayNameResolver()
        {
            Init();
        }

        private void Init()
        {

            _dict.Add(nameof(Person.GivenName), "Vorname");
            _dict.Add(nameof(Person.FamilyName), "Familienname");            
            _dict.Add(nameof(Person.Ssn), "Versicherungsnummer");
            _dict.Add(nameof(Person.Birthday), "Geburtsdatum");
            _dict.Add(nameof(Person.BirthdayD), "Geburtsdatum");
            
            _dict.Add(nameof(Person.Religion), "Religion");
            _dict.Add(nameof(Person.Insurance), "Versicherung");
            _dict.Add(nameof(Person.Nationality), "Staatsangehörigkeit");
            _dict.Add(nameof(Person.CareAllowance), "Pflegegeld");
            _dict.Add(nameof(Person.Postcode), "Plz");
            _dict.Add(nameof(Person.City), "Ort");
            _dict.Add(nameof(Person.Gender), "Geschlecht");

            _dict.Add(nameof(Activity.Amount), "Anzahl");
            _dict.Add(nameof(Activity.Date), "Datum");
            _dict.Add(nameof(Activity.DateD), "Datum");
            _dict.Add(nameof(Activity.Type), "Art");


            _dict.Add(nameof(HkpvReport.To), "Bis");
            _dict.Add(nameof(HkpvReport.ToD), "Bis");

            _dict.Add(nameof(HkpvReport.From), "Von");
            _dict.Add(nameof(HkpvReport.FromD), "Von");

            _dict.Add(nameof(HkpvReport.Institution), "Einrichtung");

        }

        public string GetDisplayName(string name)
        {            
            if (!string.IsNullOrEmpty(name) && _dict.TryGetValue(name, out string value))
                return value;

            return name;
        }

    }
}
