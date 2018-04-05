using System;
using Vodamep.Model;
using Vodamep.TestData;
using Xunit;

namespace Vodamep.Hkpv.Validation.Tests
{
    public class PersonValidationTests : ValidationTestsBase
    {
        [Fact]
        public void From_IsNotFirstDayInMonth_ReturnsError()
        {
            this.Report.From = this.Report.From.AddDays(1);

            this.AssertError("'From' muss der erste Tag des Monats sein.");
        }

        [Fact]
        public void To_IsNull_ReturnsError()
        {
            this.Report.To = null;

            this.AssertError("'To' darf keinen Null-Wert aufweisen.");
        }

        [Fact]
        public void To_IsNotLastDayInMonth_ReturnsError()
        {
            this.Report.To = this.Report.To.AddDays(-1);

            this.AssertError("'To' muss der letzte Tag des Monats sein.");
        }

        [Fact]
        public void FromTo_MoreThanOneMonth_ReturnsError()
        {
            this.Report.From = this.Report.From.AddMonths(-1);

            this.AssertError("Die Meldung muss genau einen Monat beinhalten.");
        }

        [Fact]
        public void To_IsInFuture_ReturnsError()
        {
            this.Report.From = LocalDate.Today.FirstDateInMonth();
            this.Report.To = this.Report.From.LastDateInMonth();

            this.AssertErrorRegExp("Der Wert von 'To' muss kleiner oder gleich");
        }

        [Fact]
        public void BirthDay_IsNull_ReturnsError()
        {
            this.AddPerson()
                .ManipulateData(d => d.Birthday, null);

            this.AssertError("'Geburtsdatum' darf keinen Null-Wert aufweisen.");
        }

        [Fact]
        public void BirthDay_IsInFuture_ReturnsError()
        {
            var date = DateTime.Today.AddDays(1);

            this.AddPerson()
                .ManipulateData(d => d.Birthday, new LocalDate(date))
                .ManipulateData(d => d.Ssn, DataGenerator.CreateRandomSSN(date));

            this.AssertError("'Geburtsdatum' darf nicht in der Zukunft liegen.");
        }

        [Fact]
        public void BirthDay_Before1900_ReturnsError()
        {
            var date = new DateTime(1900, 1, 1).AddDays(-1);

            this.AddPerson()
                .ManipulateData(d => d.Birthday, new LocalDate(date))
                .ManipulateData(d => d.Ssn, DataGenerator.CreateRandomSSN(date));


            this.AssertError(@"Der Wert von 'Geburtsdatum' muss grösser oder gleich '1900-01-01' sein.");
        }

        [Fact]
        public void BirthDay_BirtdayNotEqualsSSN_ReturnsError()
        {
            var date1 = new DateTime(1966, 01, 03);
            var date2 = new DateTime(1966, 03, 01);

            this.AddPerson()
                .ManipulateData(x => x.Birthday, new LocalDate(date1))
                .ManipulateData(x => x.Ssn, DataGenerator.CreateRandomSSN(date2));

            this.AssertError("Das Geburtsdatum 03.01.1966 unterscheidet sich vom Wert in der Versicherungsnummer 01.03.66.");
        }

        [Fact]
        public void SSN_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                 .ManipulateData(x => x.Ssn, string.Empty);

            this.AssertError("'Versicherungsnummer' darf nicht leer sein.");
        }

        [Fact]
        public void SSN_NotValid_ReturnsError()
        {
            this.AddPerson()
                 .ManipulateData(x => x.Ssn, "9999-23.10.54");

            this.AssertError("Die Versicherungsnummer 9999-23.10.54 ist nicht korrekt.");
        }

        [Fact]
        public void Activity_Date_IsInFurture_ReturnsError()
        {
            this.AddActivity("02,15", LocalDate.Today.AddDays(1));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss kleiner oder gleich (.*) sein");
        }

        [Fact]
        public void Activity_02Without15_ReturnsError()
        {
            this.AddActivity("02");

            this.AssertErrorRegExp("Kein Eintrag '15'");
        }

        [Fact]
        public void Activity_AnyWithout02_ReturnsError()
        {
            this.AddActivity("15");

            this.AssertErrorRegExp("Kein Eintrag '1,2,3'");
        }

        [Fact]
        public void Activity_MultipleEntriesSameStaffAndPersonAndDateAndType_ReturnsError()
        {
            this.AddActivity("02,15");
            this.AddActivity("15");

            this.AssertErrorRegExp("Die Einträge sind nicht kumuliert.");
        }

        [Fact]
        public void Activity_WithoutEntryInPersons_ReturnsError()
        {
            this.AddStaff();

            this.Report.Activities.Add(new Model.Activity() { PersonId = "1", Amount = 1, StaffId = this.Report.Staffs[0].Id, Date = LocalDate.Today, Type = Model.ActivityType.Lv02 });
            this.Report.Activities.Add(new Model.Activity() { PersonId = "1", Amount = 1, StaffId = this.Report.Staffs[0].Id, Date = LocalDate.Today, Type = Model.ActivityType.Lv15 });

            this.AssertErrorRegExp(@"Der Id '(.+)' fehlt");
        }

        [Fact]
        public void Activity_WithoutEntryInStaffs_ReturnsError()
        {
            this.AddActivities();

            this.Report.Staffs.Clear();

            this.AssertErrorRegExp(@"Der Id (.+) fehlt");
        }

        [Fact]
        public void Activities_ByTrainee_Contains06To10_ReturnsError()
        {
            this.AddActivity("02,06,15");

            this.Report.Staffs[0].Role = Model.StaffRole.Trainee;

            this.AssertErrorRegExp("darf als Auszubildende/r keine medizinischen Leistungen");
        }


        [Fact]
        public void Activities_DateIsNull_ReturnsError()
        {
            this.AddActivity("02,15");

            this.Report.Activities[0].Date = null;

            this.AssertError("'Datum' darf keinen Null-Wert aufweisen.");
        }

        [Fact]
        public void Activities_DateIsGreaterThanReportRange_ReturnsError()
        {
            this.AddActivity("02,15", new LocalDate(this.Report.To.AddDays(1)));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss kleiner oder gleich");
        }

        [Fact]
        public void Activities_DateIsLessThanReportRange_ReturnsError()
        {
            this.AddActivity("02,15", new LocalDate(this.Report.From.AddDays(-1)));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss grösser oder gleich");
        }

        [Fact]
        public void Staff_IdIsNotUnique_ReturnsError()
        {
            this.AddActivity("02,15");
            this.Report.Staffs.Add(this.Report.Staffs[0].Clone());

            this.Report.Staffs[0].Id = this.Report.Staffs[1].Id;

            this.AssertError("Der Id ist nicht eindeutig.");
        }


        [Fact]
        public void Staff_WithoutActivity_ReturnsError()
        {
            this.AddStaff();

            this.AssertErrorRegExp("Keine Aktivitäten");
        }

        [Fact]
        public void Person_WithoutActivity_ReturnsError()
        {
            this.AddPerson();

            this.AssertError("Keine Aktivitäten.");
        }

        [Fact]
        public void Persons_IdIsNotUnique_ReturnsError()
        {
            this.AddActivity("02,15");
            this.Report.Persons.Add(this.Report.Persons[0].Clone());

            this.AssertError("Der Id ist nicht eindeutig.");
        }

        [Fact]
        public void PersonalData_IdIsNotUnique_ReturnsError()
        {
            this.AddActivity("02,15");

            this.Report.PersonalData.Add(this.Report.PersonalData[0].Clone());

            this.AssertError("Der Id ist nicht eindeutig.");
        }

        [Fact]
        public void PersonalData_SSNIsNotUnique_ReturnsError()
        {
            this.AddPersons(2);
            this.AddActivities();

            this.Report.PersonalData[1].Ssn = this.Report.PersonalData[0].Ssn;
            this.Report.PersonalData[1].Birthday = this.Report.PersonalData[0].Birthday;

            this.AssertErrorRegExp("Mehrere Personen haben die selbe Versicherungsnummer");
        }

        [Fact]
        public void Consultations_31without32_ReturnsError()
        {
            this.AddConsultation("31");

            this.AssertErrorRegExp("Kein Eintrag 'Lv32' vorhanden");
        }

        [Fact]
        public void Consultations_32without321ReturnsError()
        {
            this.AddConsultation("32");

            this.AssertErrorRegExp("Kein Eintrag 'Lv31' vorhanden");
        }

        [Fact]
        public void Consultations_33without34_ReturnsError()
        {
            this.AddConsultation("33");

            this.AssertErrorRegExp("Kein Eintrag 'Lv34' vorhanden");
        }

        [Fact]
        public void Consultations_34without3_ReturnsError()
        {
            this.AddConsultation("34");

            this.AssertErrorRegExp("Kein Eintrag 'Lv33' vorhanden");
        }

        [Fact]
        public void Consultations_DateIsNull_ReturnsError()
        {
            this.AddConsultation("31,32");

            this.Report.Consultations[0].Date = null;

            this.AssertError("'Datum' darf keinen Null-Wert aufweisen.");
        }

        [Fact]
        public void Consultations_DateIsGreaterThanReportRange_ReturnsError()
        {
            this.AddConsultation("31,32", new LocalDate(this.Report.To.AddDays(1)));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss kleiner oder gleich");
        }

        [Fact]
        public void Consultations_DateIsLessThanReportRange_ReturnsError()
        {
            this.AddConsultation("31,32", new LocalDate(this.Report.From.AddDays(-1)));

            this.AssertErrorRegExp("Der Wert von 'Datum' muss grösser oder gleich");
        }

        [Fact]
        public void From_IsNull_ReturnsError()
        {
            this.Report.From = null;

            this.AssertError("'From' darf keinen Null-Wert aufweisen.");
        }

        [Fact]
        public void Religion_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                .ManipulatePerson(d => d.Religion, string.Empty);

            this.AssertError("'Religion' darf nicht leer sein.");
        }

        [Fact]
        public void Religion_CodeIsNotValid_ReturnsError()
        {
            this.AddPerson()
                .ManipulatePerson(d => d.Religion, "r.k.");

            this.AssertError("Für 'Religion' ist 'r.k.' kein gültiger Code.");
        }

        [Fact]
        public void Insurance_CodeIsNotValid_ReturnsError()
        {
            this.AddPerson()
                .ManipulatePerson(d => d.Insurance, "VGKK");

            this.AssertError("Für 'Versicherung' ist 'VGKK' kein gültiger Code.");
        }

        [Fact]
        public void Insurance_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                .ManipulatePerson(d => d.Insurance, string.Empty);

            this.AssertError("'Versicherung' darf nicht leer sein.");
        }

        [Fact]
        public void Nationality_CodeIsNotValid_ReturnsError()
        {
            this.AddPerson()
                .ManipulatePerson(d => d.Nationality, "Österreich");

            this.AssertError("Für 'Staatsangehörigkeit' ist 'Österreich' kein gültiger Code.");
        }

        [Fact]
        public void Nationality_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                .ManipulatePerson(d => d.Nationality, string.Empty);

            this.AssertError("'Staatsangehörigkeit' darf nicht leer sein.");
        }


        [Fact]
        public void City_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                .ManipulateData(d => d.City, string.Empty);

            this.AssertError("'Ort' darf nicht leer sein.");
        }

        [Fact]
        public void Postcode_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                .ManipulateData(d => d.Postcode, string.Empty);

            this.AssertError("'Plz' darf nicht leer sein.");
        }

        [Fact]
        public void Street_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                .ManipulateData(d => d.Street, string.Empty);

            this.AssertError("'Anschrift' darf nicht leer sein.");
        }

        [Fact]
        public void Country_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                .ManipulateData(d => d.Country, string.Empty);

            this.AssertError("'Land' darf nicht leer sein.");
        }

        [Fact]
        public void Country_CodeIsNotValid_ReturnsError()
        {
            this.AddPerson()
                .ManipulateData(d => d.Country, "Österreich");

            this.AssertError("Für 'Land' ist 'Österreich' kein gültiger Code.");
        }

        [Fact]
        public void FamilyName_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                .ManipulateData(d => d.FamilyName, string.Empty);

            this.AssertError("'Familienname' darf nicht leer sein.");
        }

        [Fact]
        public void GivenName_IsEmpty_ReturnsError()
        {
            this.AddPerson()
                .ManipulateData(d => d.GivenName, string.Empty);

            this.AssertError("'Vorname' darf nicht leer sein.");
        }
    }
}
