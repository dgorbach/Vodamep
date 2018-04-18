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
                var name = GetName(a.Name_1);
                report.AddPerson((new Person()
                {
                    Id = GetId(a.Adressnummer),
                    Insurance = a.Versicherung ?? string.Empty,
                    Nationality = a.Staatsbuergerschaft ?? string.Empty,
                    CareAllowance = (CareAllowance)a.Pflegestufe,
                    Religion = ReligionCodeProvider.Instance.Unknown,
                    Postcode = a.Postleitzahl ?? string.Empty,
                    City = a.Ort ?? string.Empty,
                    Gender = GetGender(a.Geschlecht)
                }, new PersonalData()
                {
                    Id = GetId(a.Adressnummer),
                    BirthdayD = a.Geburtsdatum,
                    FamilyName = name.Familyname,
                    GivenName = name.Givenname,
                    Ssn = a.Versicherungsnummer ?? string.Empty,
                    Street = a.Adresse ?? string.Empty,

                }));
            }


            foreach (var p in data.P)
            {
                var name = GetName(p.Pflegername);

                report.Staffs.Add(new Staff() { Id = GetId(p.Pflegernummer), FamilyName = name.Familyname, GivenName = name.Givenname });
            }

            foreach (var l in data.L)
            {
                if (l.Leistung < 20)
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
                else
                {
                    report.Consultations.Add(new Consultation()
                    {
                        Amount = l.Anzahl,
                        DateD = l.Datum,
                        StaffId = GetId(l.Pfleger),
                        Type = (ConsultationType)l.Leistung
                    });
                }
            }

            var filename = report.WriteToPath("", asJson: asJson, compressed: true);

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
                names.Add("??");

            return (names[0], string.Join(" ", names.Skip(1)));

        }

        private string GetId(int id) => $"{id}";
    }
}
