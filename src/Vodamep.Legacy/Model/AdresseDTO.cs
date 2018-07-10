using System;

namespace Vodamep.Legacy.Model
{
    public class AdresseDTO
    {
        public int Adressnummer { get; set; }
        public string Name_1 { get; set; }
        public string Name_2 { get; set; }

        public string Postleitzahl { get; set; }

        public string Ort { get; set; }

        public DateTime Geburtsdatum { get; set; }

        public string Staatsbuergerschaft { get; set; } = Data.CountryCodeProvider.Instance.Unknown;

        public string Versicherung { get; set; } = Data.InsuranceCodeProvider.Instance.Unknown;

        public string Versicherungsnummer { get; set; }

        public string Religion { get; set; } = Data.ReligionCodeProvider.Instance.Unknown;

        public int Pflegestufe { get; set; }

        public string Geschlecht { get; set; }
    }
}
