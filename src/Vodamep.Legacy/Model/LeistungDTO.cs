using System;

namespace Vodamep.Legacy.Model
{
    public class LeistungDTO
    {
        public int Adressnummer { get; set; }
        public int Pfleger { get; set; }

        public DateTime Datum { get; set; }

        public int Leistung { get; set; }
        public int Anzahl { get; set; }

    }
}
