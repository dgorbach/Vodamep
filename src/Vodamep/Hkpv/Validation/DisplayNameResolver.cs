using System.Collections.Generic;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class DisplayNameResolver
    {
        private readonly IDictionary<string, string> _dict = new SortedDictionary<string, string>();

        public DisplayNameResolver()
        {
            Init();
        }

        private void Init()
        {

            _dict.Add(nameof(PersonalData.GivenName), "Vorname");
            _dict.Add(nameof(PersonalData.FamilyName), "Familienname");
            _dict.Add(nameof(PersonalData.Street), "Anschrift");
            _dict.Add(nameof(PersonalData.Postcode), "Plz");
            _dict.Add(nameof(PersonalData.Ssn), "Versicherungsnummer");
            _dict.Add(nameof(PersonalData.City), "Ort");
            _dict.Add(nameof(PersonalData.Country), "Land");
            _dict.Add(nameof(PersonalData.Birthday), "Geburtsdatum");


            _dict.Add(nameof(Person.Religion), "Religion");
            _dict.Add(nameof(Person.Insurance), "Versicherung");
            _dict.Add(nameof(Person.Nationality), "Staatsangehörigkeit");
            

            _dict.Add(nameof(Activity.Amount), "Anzahl");
            _dict.Add(nameof(Activity.Date), "Datum");

        }

        public string GetDisplayName(string name)
        {            
            if (!string.IsNullOrEmpty(name) && _dict.TryGetValue(name, out string value))
                return value;

            return name;
        }

    }
}
