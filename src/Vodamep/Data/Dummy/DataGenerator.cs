using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;

namespace Vodamep.Data.Dummy
{
    internal class DataGenerator
    {

        private static DataGenerator _instance;

        public static DataGenerator Instance
        {
            get
            {

                if (_instance == null)
                    _instance = new DataGenerator();

                return _instance;
            }
        }


        private long _id = 1;
        private Random _rand = new Random();
        private string[] _addresses;
        private string[] _names;
        private string[] _familynames;
        private string[] _activities;
        private CareAllowance[] _careAllowances = new[] { CareAllowance.L1, CareAllowance.L2, CareAllowance.L3,
                CareAllowance.L4, CareAllowance.L5, CareAllowance.L5, CareAllowance.L7,
                CareAllowance.Any, CareAllowance.Unknown };


        private DataGenerator()
        {
            _addresses = ReadRessource("gemplzstr_8.csv").ToArray();
            _names = ReadRessource("Vornamen.txt").ToArray();
            _familynames = ReadRessource("Nachnamen.txt").ToArray();
            _activities = ReadRessource("Aktivitäten.txt").ToArray();
        }


        private IEnumerable<string> ReadRessource(string name)
        {
            var assembly = this.GetType().Assembly;
            var resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Data.Dummy.{name}");

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }

        public HkpvReport CreateHkpvReport(int? year = null, int? month = null, int persons = 100, int staffs = 5, bool addActivities = true)
        {
            var report = new HkpvReport()
            {
                Institution = new Institution() { Id = "kpv_test", Name = "Testverein" }
            };

            var from = year.HasValue || month.HasValue ? new DateTime(year ?? DateTime.Today.Year, month ?? DateTime.Today.Month, 1) : DateTime.Today.FirstDateInMonth().AddMonths(-1);


            report.FromD = from;
            report.ToD = report.FromD.LastDateInMonth();

            report.AddDummyPersons(persons);
            report.AddDummyStaffs(staffs);

            if (addActivities)
                report.AddDummyActivities();

            return report;
        }

        public (Person Person, PersonalData data) CreatePerson()
        {
            var id = (_id++).ToString();

            var person = new Person()
            {
                Id = id,
                Insurance = "19",
                Religion = "VAR",
                Nationality = "AT",
                CareAllowance = _careAllowances[_rand.Next(_careAllowances.Length)],
                Gender = _rand.Next(2) == 1 ? Gender.Female : Gender.Male
            };

            var data = new PersonalData
            {
                Id = id,
                FamilyName = _familynames[_rand.Next(_familynames.Length)],
                GivenName = _names[_rand.Next(_names.Length)]
            };

            // die Anschrift
            {
                var address = _addresses[_rand.Next(_addresses.Length)].Split(';');

                person.Postcode = address[6];
                person.City = address[3];
                data.Street = string.Format("{0} {1}", address[5], 1 + _rand.Next(30));
            }

            data.BirthdayD = new DateTime(1920, 01, 01).AddDays(_rand.Next(20000));

            data.Ssn = CreateRandomSSN(data.BirthdayD);

            return (person, data);

        }
        public IEnumerable<(Person Person, PersonalData data)> CreatePersons(int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreatePerson();
        }


        public string CreateRandomSSN(DateTime date)
        {
            int nr;
            int cd;

            while (true)
            {
                nr = _rand.Next(899) + 100;

                cd = SSNHelper.GetCheckDigit(nr.ToString("000"), date.ToString("ddMMyy"));

                if (cd >= 0 && cd <= 9)
                    break;
            }

            return SSNHelper.Format(string.Format("{0}{1}-{2:dd.MM.yy}", nr, cd, date));
        }

        public Staff CreateStaff()
        {
            var id = (_id++).ToString();

            var staff = new Staff
            {
                Id = id,
                FamilyName = _familynames[_rand.Next(_familynames.Length)],
                GivenName = _names[_rand.Next(_names.Length)]
            };

            return staff;
        }

        public IEnumerable<Staff> CreateStaffs(int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreateStaff();
        }

        private ActivityType[] CreateRandomActivities()
        {
            var a = _activities[_rand.Next(_activities.Length)];

            if (string.IsNullOrEmpty(a)) return new ActivityType[0];

            return a.Split(',').Select(x => (ActivityType)int.Parse(x)).ToArray();
        }

        private IEnumerable<Activity> CreateRandomActivity(string personId, string staffId, DateTime date)
        {
            var activities = CreateRandomActivities();

            foreach (var entry in activities.GroupBy(x => x))
            {
                yield return new Activity()
                {
                    StaffId = staffId,
                    PersonId = personId,
                    DateD = date,
                    Amount = entry.Count(),
                    Type = entry.Key
                };
            }
        }

        public Activity[] CreateActivities(HkpvReport report)
        {
            var result = new List<Activity>();

            foreach (var staff in report.Staffs)
            {
                // die zu betreuenden Personen zufällig zuordnen
                var persons = report.Persons.Count == 1 || report.Staffs.Count == 1 ? report.Persons.ToArray() : report.Persons.Where(x => _rand.Next(report.Staffs.Count) == 0).ToArray();

                // ein Mitarbeiter soll pro Monat max. 6000 Minuten arbeiten. 
                // wenn nur wenige Personen betreut werden: max 500 Minuten pro Person
                var minuten = _rand.Next(Math.Min(persons.Count() * 500, 6000));

                while (minuten > 0)
                {
                    var personId = persons[_rand.Next(persons.Length)].Id;

                    var date = report.FromD.AddDays(_rand.Next(report.ToD.Day - report.FromD.Day + 1));

                    // Pro Tag, Person und Mitarbeiter nur ein Eintrag erlaubt:
                    if (!result.Where(x => x.PersonId == personId && x.StaffId == staff.Id && x.DateD.Equals(date)).Any())
                    {
                        var a = CreateRandomActivity(personId, staff.Id, date).ToArray();

                        minuten -= a.Select(x => x.GetMinutes()).Sum();

                        result.AddRange(a);
                    }
                }
            }

            //sicherstellen, dass jede Person zumindest einen Eintrag hat
            foreach (var p in report.Persons.Where(x => !result.Where(a => a.PersonId == x.Id).Any()).ToArray())
            {
                var date = report.FromD.AddDays(_rand.Next(report.ToD.Day - report.FromD.Day + 1));

                result.AddRange(CreateRandomActivity(p.Id, report.Staffs[_rand.Next(report.Staffs.Count)].Id, date));
            }


            return result.ToArray();
        }
    }
}
