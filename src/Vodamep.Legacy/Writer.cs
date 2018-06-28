using System;
using System.Linq;
using Vodamep.Data;
using Vodamep.Hkpv.Model;
using Vodamep.Legacy.Reader;

namespace Vodamep.Legacy
{
    public class Writer
    {
        public string Write(ReadResult data, bool asJson = false)
        {
            var date = data.L.Select(x => x.Datum).First().FirstDateInMonth();
            var report = new HkpvReport()
            {
                FromD = date,
                ToD = date.LastDateInMonth(),
                Institution = new Institution() { Id = data.V.Vereinsnummer.ToString(), Name = data.V.Bezeichnung }
            };
            foreach (var a in data.A)
            {
                var name = (Familyname: a.Name_1, Givenname: a.Name_2);
                if (string.IsNullOrEmpty(name.Givenname)) name = GetName(a.Name_1);

                var postcodeCity = GetPostCodeCity(a);

                report.AddPerson(new Person()
                {
                    Id = GetId(a.Adressnummer),
                    BirthdayD = a.Geburtsdatum,
                    FamilyName = (name.Familyname ?? string.Empty).Trim(),
                    GivenName = (name.Givenname ?? string.Empty).Trim(),
                    Ssn = (a.Versicherungsnummer ?? string.Empty).Trim(),
                    Insurance = (a.Versicherung ?? string.Empty).Trim(),
                    Nationality = (a.Staatsbuergerschaft ?? string.Empty).Trim(),
                    CareAllowance = (CareAllowance)a.Pflegestufe,
                    Religion = ReligionCodeProvider.Instance.Unknown,
                    Postcode = postcodeCity.Postcode,
                    City = postcodeCity.City,
                    Gender = GetGender(a.Geschlecht)
                });
            }

            foreach (var p in data.P)
            {
                var name = GetName(p.Pflegername);

                report.Staffs.Add(new Staff() { Id = GetId(p.Pflegernummer), FamilyName = name.Familyname, GivenName = name.Givenname });
            }

            foreach (var l in data.L)
            {
                if (Activity.ActivityTypesWithoutPerson.Contains((ActivityType)l.Leistung))
                {
                    report.Activities.Add(new Activity()
                    {
                        Amount = l.Anzahl,
                        DateD = l.Datum,
                        PersonId = string.Empty,
                        StaffId = GetId(l.Pfleger),
                        Type = (ActivityType)l.Leistung
                    });
                }
                else
                {
                    report.Activities.Add(new Activity()
                    {
                        Amount = l.Anzahl,
                        DateD = l.Datum,
                        PersonId = GetId(l.Adressnummer),
                        StaffId = GetId(l.Pfleger),
                        Type = (ActivityType)l.Leistung
                    });

                }
            }

            var filename = report.AsSorted().WriteToPath("", asJson: asJson, compressed: true);

            return filename;
        }


        private Gender GetGender(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Gender.UndefinedGender;

            switch (value.ToLower().Substring(0, 1))
            {
                case "m":
                    return Gender.Male;
                case "w":
                case "f":
                    return Gender.Female;

                default:
                    return Gender.UndefinedGender;
            }
        }

        private (string Familyname, string Givenname) GetName(string name)
        {
            var names = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            while (names.Count < 2)
                names.Add("- - -");

            return (names[0], string.Join(" ", names.Skip(1)));

        }

        private string GetId(int id) => $"{id}";

        private (string Postcode, string City) GetPostCodeCity(Model.AdresseDTO a)
        {

            var plz = (a.Postleitzahl ?? string.Empty).Trim();
            var ort = (a.Ort ?? string.Empty).Trim();
                        
            if (plz.Length > 4)
            {
                // im Datenbestand gibt es "gebastelte" Postleitzahlen mit 6 Zeichen
                // z.b. 690060 Möggers
                // entweder nur die ersten vier Zeichen verwenden, oder die PLZ anhand des Ortsnames ermitteln

                if (Postcode_CityProvider.Instance.IsValid($"{plz.Substring(0, 4)} {ort}"))
                {
                    plz = plz.Substring(0, 4);
                }
                else
                {
                    var plz2 = Postcode_CityProvider.Instance.GetCSV().Where(x => x.EndsWith($" {ort};")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(plz2))
                        plz = plz2;
                }
            }

            return (plz, ort);
        }
    }
}
