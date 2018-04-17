using System;
using Vodamep.Data.Dummy;
using Vodamep.Hkpv.Model;
using Xunit;

namespace Vodamep.Hkpv.Validation.Tests
{
    public class PersonValidationTests : ValidationTestsBase
    {
        [Fact]
        public void From_IsNotFirstDayInMonth_ReturnsError()
        {
            this.Report.FromD = this.Report.FromD.AddDays(1);

            this.AssertValidation("'Von' muss der erste Tag des Monats sein.");
        }

        [Fact]
        public void To_IsEmpty_ReturnsError()
        {
            this.Report.To = string.Empty;

            this.AssertValidation("'Bis' darf nicht leer sein.");
        }

        [Fact]
        public void To_IsNotLastDayInMonth_ReturnsError()
        {
            this.Report.ToD = this.Report.ToD.AddDays(-1);

            this.AssertValidation("'Bis' muss der letzte Tag des Monats sein.");
        }

        [Fact]
        public void FromTo_MoreThanOneMonth_ReturnsError()
        {
            this.Report.FromD = this.Report.FromD.AddMonths(-1);

            this.AssertValidation("Die Meldung muss genau einen Monat beinhalten.");
        }

        [Fact]
        public void To_IsInFuture_ReturnsError()
        {
            this.Report.FromD = DateTime.Today.FirstDateInMonth();
            this.Report.ToD = this.Report.FromD.LastDateInMonth();

            this.AssertErrorRegExp("Der Wert von 'Bis' muss kleiner oder gleich");
        }

        [Fact]
        public void BirthDay_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulateData(d => d.Birthday, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Geburtsdatum' darf nicht leer sein.");
        }

        [Fact]
        public void BirthDay_IsInFuture_ReturnsError()
        {
            var date = DateTime.Today.AddDays(1);

            this.Report.AddDummyPerson()
                .ManipulateData(d => d.BirthdayD, date)
                .ManipulateData(d => d.Ssn, DataGenerator.Instance.CreateRandomSSN(date));

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Geburtsdatum' darf nicht in der Zukunft liegen.");
        }

        [Fact]
        public void BirthDay_Before1900_ReturnsError()
        {
            var date = new DateTime(1900, 1, 1).AddDays(-1);

            this.Report.AddDummyPerson()
                .ManipulateData(d => d.BirthdayD, date)
                .ManipulateData(d => d.Ssn, DataGenerator.Instance.CreateRandomSSN(date));

            this.Report.AddDummyActivity("02,15");

            this.AssertErrorRegExp(@"Der Wert von 'Geburtsdatum' muss grösser oder gleich");
        }

        [Fact]
        public void BirthDay_BirtdayNotEqualsSSN_ReturnsWarning()
        {
            var date1 = new DateTime(1966, 01, 03);
            var date2 = new DateTime(1966, 03, 01);

            this.Report.AddDummyPerson()
                .ManipulateData(x => x.BirthdayD, date1)
                .ManipulateData(x => x.Ssn, DataGenerator.Instance.CreateRandomSSN(date2));

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("Das Geburtsdatum 03.01.1966 unterscheidet sich vom Wert in der Versicherungsnummer 01.03.66.", FluentValidation.Severity.Warning);
        }

        [Fact]
        public void SSN_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                 .ManipulateData(x => x.Ssn, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Versicherungsnummer' darf nicht leer sein.");
        }

        [Fact]
        public void SSN_NotValid_ReturnsError()
        {
            this.Report.AddDummyPerson()
                 .ManipulateData(x => x.Ssn, "9999-23.10.54");

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("Die Versicherungsnummer 9999-23.10.54 ist nicht korrekt.");
        }

        [Fact]
        public void Activity_Date_IsInFurture_ReturnsError()
        {
            this.Report.AddDummyActivity("02,15", DateTime.Today.AddDays(1));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss kleiner oder gleich (.*) sein");
        }

        [Fact]
        public void Activity_02Without15_ReturnsError()
        {
            this.Report.AddDummyActivity("02");

            this.AssertErrorRegExp("Kein Eintrag '15'");
        }

        [Fact]
        public void Activity_AnyWithout02_ReturnsError()
        {
            this.Report.AddDummyActivity("15");

            this.AssertErrorRegExp("Kein Eintrag '1,2,3'");
        }

        [Fact]
        public void Activity_MultipleEntriesSameStaffAndPersonAndDateAndType_ReturnsError()
        {
            this.Report.AddDummyActivity("02,15");
            this.Report.AddDummyActivity("15");

            this.AssertErrorRegExp("Die Einträge sind nicht kumuliert.");
        }

        [Fact]
        public void Activity_WithoutEntryInPersons_ReturnsError()
        {
            this.Report.AddDummyStaff();

            this.Report.Activities.Add(new Activity() { PersonId = "1", Amount = 1, StaffId = this.Report.Staffs[0].Id, DateD = DateTime.Today, Type = ActivityType.Lv02 });
            this.Report.Activities.Add(new Activity() { PersonId = "1", Amount = 1, StaffId = this.Report.Staffs[0].Id, DateD = DateTime.Today, Type = ActivityType.Lv15 });

            this.AssertErrorRegExp(@"Der Id '(.+)' fehlt");
        }

        [Fact]
        public void Activity_WithoutEntryInStaffs_ReturnsError()
        {
            this.Report.AddDummyActivities();

            this.Report.Staffs.Clear();

            this.AssertErrorRegExp(@"Der Id (.+) fehlt");
        }

        [Fact]
        public void Activities_ByTrainee_Contains06To10_ReturnsError()
        {
            this.Report.AddDummyActivity("02,06,15");

            this.Report.Staffs[0].Role = StaffRole.Trainee;

            this.AssertErrorRegExp("darf als Auszubildende/r keine medizinischen Leistungen");
        }


        [Fact]
        public void Activities_DateIsEmpty_ReturnsError()
        {
            this.Report.AddDummyActivity("02,15");

            this.Report.Activities[0].Date = string.Empty;
            this.Report.Activities[1].Date = string.Empty;

            this.AssertValidation("'Datum' darf nicht leer sein.");
        }

        [Fact]
        public void Activities_DateIsGreaterThanReportRange_ReturnsError()
        {
            this.Report.AddDummyActivity("02,15", this.Report.ToD.AddDays(1));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss kleiner oder gleich");
        }

        [Fact]
        public void Activities_DateIsLessThanReportRange_ReturnsError()
        {
            this.Report.AddDummyActivity("02,15", this.Report.FromD.AddDays(-1));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss grösser oder gleich");
        }

        [Fact]
        public void Staff_IdIsNotUnique_ReturnsError()
        {
            this.Report.AddDummyActivity("02,15");
            this.Report.Staffs.Add(this.Report.Staffs[0].Clone());

            this.Report.Staffs[0].Id = this.Report.Staffs[1].Id;

            this.AssertValidation("Der Id ist nicht eindeutig.");
        }


        [Fact]
        public void Staff_WithoutActivity_ReturnsError()
        {
            this.Report.AddDummyStaff();

            this.AssertErrorRegExp("Keine Aktivitäten");
        }

        [Fact]
        public void Person_WithoutActivity_ReturnsError()
        {
            this.Report.AddDummyPerson();

            this.AssertValidation("Keine Aktivitäten.");
        }

        [Fact]
        public void Persons_IdIsNotUnique_ReturnsError()
        {
            var p1 = this.Report.AddDummyPerson();
            
            var p2 = this.Report.AddDummyPerson();
            p2.person.Id = p1.person.Id;
            this.Report.PersonalData.Remove(p2.Data);

            this.Report.AddDummyActivities();


            this.AssertValidation("Der Id ist nicht eindeutig.");
        }

        [Fact]
        public void PersonalData_IdIsNotUnique_ReturnsError()
        {
            var p1 = this.Report.AddDummyPerson();

            var p2 = this.Report.AddDummyPerson();
            this.Report.Persons.Remove(p2.person);
            p2.Data.Id = p1.person.Id;

            this.Report.AddDummyActivities();

            

            this.AssertValidation("Der Id ist nicht eindeutig.");
        }

        [Fact]
        public void PersonalData_SSNIsNotUnique_ReturnsError()
        {
            this.Report.AddDummyPersons(2);
            this.Report.AddDummyActivities();

            this.Report.PersonalData[1].Ssn = this.Report.PersonalData[0].Ssn;
            this.Report.PersonalData[1].BirthdayD = this.Report.PersonalData[0].BirthdayD;

            this.AssertErrorRegExp("Mehrere Personen haben die selbe Versicherungsnummer");
        }

        [Fact]
        public void Consultations_31without32_ReturnsError()
        {
            this.Report.AddDummyConsultation("31");

            this.AssertErrorRegExp("Kein Eintrag 'Lv32' vorhanden");
        }

        [Fact]
        public void Consultations_32without321ReturnsError()
        {
            this.Report.AddDummyConsultation("32");

            this.AssertErrorRegExp("Kein Eintrag 'Lv31' vorhanden");
        }

        [Fact]
        public void Consultations_33without34_ReturnsError()
        {
            this.Report.AddDummyConsultation("33");

            this.AssertErrorRegExp("Kein Eintrag 'Lv34' vorhanden");
        }

        [Fact]
        public void Consultations_34without3_ReturnsError()
        {
            this.Report.AddDummyConsultation("34");

            this.AssertErrorRegExp("Kein Eintrag 'Lv33' vorhanden");
        }

        [Fact]
        public void Consultations_DateIsEmpty_ReturnsError()
        {
            this.Report.AddDummyConsultation("31,32");

            this.Report.Consultations[0].Date = string.Empty;
            this.Report.Consultations[1].Date = string.Empty;

            this.AssertValidation("'Datum' darf nicht leer sein.");
        }

        [Fact]
        public void Consultations_DateIsGreaterThanReportRange_ReturnsError()
        {
            this.Report.AddDummyConsultation("31,32", this.Report.ToD.AddDays(1));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss kleiner oder gleich");
        }

        [Fact]
        public void Consultations_DateIsLessThanReportRange_ReturnsError()
        {
            this.Report.AddDummyConsultation("31,32", this.Report.FromD.AddDays(-1));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss grösser oder gleich");
        }

        [Fact]
        public void From_IsEmpty_ReturnsError()
        {
            this.Report.From = string.Empty;

            this.AssertValidation("'Von' darf nicht leer sein.");
        }

        [Fact]
        public void Religion_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(d => d.Religion, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Religion' darf nicht leer sein.");
        }

        [Fact]
        public void Religion_CodeIsNotValid_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(d => d.Religion, "r.k.");

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("Für 'Religion' ist 'r.k.' kein gültiger Code.");
        }

        [Fact]
        public void Insurance_CodeIsNotValid_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(d => d.Insurance, "VGKK");

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("Für 'Versicherung' ist 'VGKK' kein gültiger Code.");
        }

        [Fact]
        public void Insurance_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(d => d.Insurance, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Versicherung' darf nicht leer sein.");
        }

        [Fact]
        public void Nationality_CodeIsNotValid_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(d => d.Nationality, "Österreich");

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("Für 'Staatsangehörigkeit' ist 'Österreich' kein gültiger Code.");
        }

        [Fact]
        public void Nationality_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(d => d.Nationality, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Staatsangehörigkeit' darf nicht leer sein.");
        }

        [Fact]
        public void CareAllowence_Undefined_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(p => p.CareAllowance, CareAllowance.UndefinedAllowance);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Pflegegeld' darf nicht 'UndefinedAllowance' sein.");
        }

        [Fact]
        public void City_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(d => d.City, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Ort' darf nicht leer sein.");
        }

        [Fact]
        public void Postcode_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(d => d.Postcode, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Plz' darf nicht leer sein.");
        }

        [Fact]
        public void Postcode_City_NotInListOfValidCities_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulatePerson(d => d.Postcode, "6900")
                .ManipulatePerson(d => d.City, "Lochau");

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'6900 Lochau' ist kein gültiger Ort.");
        }

        [Fact]
        public void Street_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulateData(d => d.Street, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Anschrift' darf nicht leer sein.");
        }

        [Fact]
        public void FamilyName_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulateData(d => d.FamilyName, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Familienname' darf nicht leer sein.");
        }

        [Fact]
        public void GivenName_IsEmpty_ReturnsError()
        {
            this.Report.AddDummyPerson()
                .ManipulateData(d => d.GivenName, string.Empty);

            this.Report.AddDummyActivity("02,15");

            this.AssertValidation("'Vorname' darf nicht leer sein.");
        }

        
    }
}
