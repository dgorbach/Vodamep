using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Vodamep.Hkpv.Validation
{
    internal static class SSNHelper
    {
        private static Regex Pattern = new Regex(@"^(?<nr>\d{3})(?<cd>\d)-?(?<tt>(0?[1-9]|[12][0-9]|3[01]))\.?(?<mm>(0?[1-9]|1[0123]))\.?(?<jj>\d{1,2})$");
        public static bool IsValid(string vnummer)
        {
            if (string.IsNullOrEmpty(vnummer))
                return true;

            bool result = false;

            vnummer = Format(vnummer);

            if (!string.IsNullOrEmpty(vnummer))
            {
                var m = Pattern.Match(vnummer);
                if (m.Success)
                {
                    var numberPart = string.Format("{0:D3}", int.Parse(m.Groups["nr"].Value));
                    var birthdatePart = string.Format("{0:D2}{1:D2}{2:D2}", int.Parse(m.Groups["tt"].Value), int.Parse(m.Groups["mm"].Value), int.Parse(m.Groups["jj"].Value));

                    var cd = int.Parse(m.Groups["cd"].Value);

                    if (cd == GetCheckDigit(numberPart, birthdatePart))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static int GetCheckDigit(string numberPart, string birthdatePart)
        {
            //Berechnung der Prüfziffer (= 4. Stelle der Laufnummer):
            //Laufnummer mit 3,7,9 und Geburtsdatum mit 5,8,4,2,1,6 multiplizieren und summieren
            //Prüfzimmer = Divisionsrest bei Division durch 11
            //Beispiel: 123x10180 ... x = 7 ... berechnet mit:
            //x = (1 * 3 + 2 * 7 + 3 * 9 + 0 * 5 + 1 * 8 + 0 * 4 + 1 * 2 + 8 * 1 + 0 * 6) Modulo 11

            var checkdigit = (Convert.ToInt16(numberPart.Substring(0, 1)) * 3
                            + Convert.ToInt16(numberPart.Substring(1, 1)) * 7
                            + Convert.ToInt16(numberPart.Substring(2, 1)) * 9
                            + Convert.ToInt16(birthdatePart.Substring(0, 1)) * 5
                            + Convert.ToInt16(birthdatePart.Substring(1, 1)) * 8
                            + Convert.ToInt16(birthdatePart.Substring(2, 1)) * 4
                            + Convert.ToInt16(birthdatePart.Substring(3, 1)) * 2
                            + Convert.ToInt16(birthdatePart.Substring(4, 1)) * 1
                            + Convert.ToInt16(birthdatePart.Substring(5, 1)) * 6
                            ) % 11;

            return checkdigit;
        }

        public static string Format(string vnummer)
        {
            string result = null;

            if (!string.IsNullOrEmpty(vnummer))
            {
                var m = Pattern.Match(vnummer);
                if (m.Success)
                {
                    result = string.Format("{0}{1}-{2:00}.{3:00}.{4:00}", m.Groups["nr"].Value, m.Groups["cd"].Value, int.Parse(m.Groups["tt"].Value), int.Parse(m.Groups["mm"].Value), int.Parse(m.Groups["jj"].Value));
                }
            }

            return result;
        }

        public static DateTime? GetBirthDay(string vnummer)
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(vnummer) && IsValid(vnummer))
            {
                var m = Pattern.Match(vnummer);

                var yearWithFourDigits = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(int.Parse(m.Groups["jj"].Value));

                try
                {
                    date = new DateTime(yearWithFourDigits, int.Parse(m.Groups["mm"].Value), int.Parse(m.Groups["tt"].Value));
                }
                catch   //z.B. bei Versicherungsnummer mit unbekanntem Geburtstag und daher Monat 13
                { }

                if (date > DateTime.Today)
                {
                    date = date.Value.AddYears(-100);
                }
            }

            return date;
        }
    }
}
