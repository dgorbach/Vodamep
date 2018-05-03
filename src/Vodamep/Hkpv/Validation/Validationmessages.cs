using System.Collections.Generic;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class Validationmessages
    {  
        public static string ActivityIsNotUnique => $"Die Einträge sind nicht kumuliert.";
        public static string ActivityMoreThenFive => $"Es wurden mehr als 5 gemeldet.";
        public static string TraineeMustNotContain06To10(Staff staff) => $"'{staff.FamilyName} {staff.GivenName} ({staff.Id})' darf als Auszubildende/r keine medizinischen Leistungen (6-10) dokumentieren.";
        public static string IdIsNotUnique => "Der Id ist nicht eindeutig.";
        public static string IdIsMissing(string id) => $"Der Id '{id}' fehlt.";
        public static string PersonWithoutPersonalDate => "Kein Personenbezug vorhanden";
        public static string PersonWithoutData => "Keine Stammdaten vorhanden";
        public static string WithoutEntry(string e) => $"Kein Eintrag '{e}' vorhanden.";
        public static string WithoutActivity => $"Keine Aktivitäten.";
        public static string BirthdayNotInFuture => "'Geburtsdatum' darf nicht in der Zukunft liegen.";
        public static string BirthdayNotInSsn(PersonalData data) => $"Das Geburtsdatum {data.BirthdayD.ToString("dd.MM.yyyy")} unterscheidet sich vom Wert in der Versicherungsnummer {SSNHelper.Format(data.Ssn).Substring(5)}.";
        public static string SsnNotValid => "Die Versicherungsnummer {PropertyValue} ist nicht korrekt.";
        public static string SsnNotUnique(IEnumerable<PersonalData> p) => $"Mehrere Personen haben die selbe Versicherungsnummer {p.First().Ssn}: {string.Join(", ", p.Select(x => $"{x.GivenName} {x.FamilyName}({x.Id})"))}";
        public static string LocalDate => "'{PropertyName}': '{PropertyValue}' ist kein gültiges Datum.";
        public static string OneMonth => "Die Meldung muss genau einen Monat beinhalten.";
        public static string LastDateInMonth => "'{PropertyName}' muss der letzte Tag des Monats sein.";
        public static string FirstDateInMOnth => "'{PropertyName}' muss der erste Tag des Monats sein.";
        public static string InvalidCode => "Für '{PropertyName}' ist '{PropertyValue}' kein gültiger Code.";
        public static string InvalidPostCode_City => "'{PropertyValue}' ist kein gültiger Ort.";
    }
}
