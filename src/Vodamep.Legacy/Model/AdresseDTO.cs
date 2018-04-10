using System;

namespace Vodamep.Legacy.Model
{
    public class AdresseDTO
    {
        public int Adressnummer { get; set; }
        public string Name_1 { get; set; }
        public string Name_2 { get; set; }

        public string Adresse { get; set; }

        public string Postleitzahl { get; set; }

        public string Ort { get; set; }

        public DateTime Geburtsdatum { get; set; }

        public string Staatsbuergerschaft { get; set; }

        public string Versicherung { get; set; }

        public string Versicherungsnummer { get; set; }

    }
}
