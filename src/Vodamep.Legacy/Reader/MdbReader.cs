using Dapper;
using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Vodamep.Legacy.Model;

namespace Vodamep.Legacy.Reader
{
    public class MdbReader : IReader
    {
        private readonly string _connectionstring;

        public MdbReader(string file)
        {
            _connectionstring = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={file}";
        }

        public ReadResult Read(int year, int month)
        {
            using (var connection = new OleDbConnection(_connectionstring))
            {
                connection.Open();

                var from = new DateTime(year, month, 1);
                var to = from.LastDateInMonth().Date.AddHours(23).AddMinutes(59);

                var sqlLeistungen = @"SELECT l.Adressnummer, l.Pfleger, l.Datum, l.Leistung, l.Anzahl
FROM Leistungen AS l
WHERE(l.Datum Between @from And @to) and not (l.Leistung in (32,34));";

                var leistungen = connection.Query<LeistungDTO>(sqlLeistungen, new { from = from, to = to }).ToArray();

                if (!leistungen.Any())
                    return ReadResult.Empty;

                var adressnummern = leistungen.Where(x => x.Leistung < 20).Select(x => x.Adressnummer).Distinct().ToArray();

                var sqlAdressen = @"SELECT a.Adressnummer, a.Name_1, a.Name_2, a.Land, a.Postleitzahl, a.Geburtsdatum, a.Staatsbuergerschaft, a.Versicherung, a.Versicherungsnummer, o.Ort, a.Geschlecht
FROM Adressen AS a LEFT JOIN tb_orte AS o  ON o.Postleitzahl = a.Postleitzahl 
where a.Adressnummer in @ids;";

                var adressen = connection.Query<AdresseDTO>(sqlAdressen, new { ids = adressnummern }).ToArray();

                var pflegestufenSql = @"SELECT d.Adressnummer as Item1, First(d.Detail)  AS Item2
FROM (

SELECT d0.Adressnummer, d0.Detail FROM Doku as d0
WHERE d0.Gruppe='21' AND d0.Datum<=@to AND d0.Adressnummer in @ids 
ORDER BY d0.Datum DESC
) as d
GROUP BY d.Adressnummer;
                ";


                var pflegestufen = connection.Query<(int Adressnummer, string Wert)>(pflegestufenSql, new { to = to, ids = adressnummern }).ToArray();

                foreach (var a in adressen)
                {
                    var ps = pflegestufen.Where(x => x.Adressnummer == a.Adressnummer).Select(x => x.Wert).FirstOrDefault();

                    if (string.IsNullOrEmpty(ps))
                        a.Pflegestufe = (int)Hkpv.Model.CareAllowance.Unknown;
                    else
                        a.Pflegestufe = int.Parse(ps);
                }

                var pflegernummern = leistungen.Select(x => x.Pfleger).Distinct().ToArray();

                var sqlPfleger = @"SELECT p.Pflegernummer, p.Pflegername FROM Pfleger AS p where p.Pflegernummer in @ids;";

                var pfleger = connection.Query<PflegerDTO>(sqlPfleger, new { ids = pflegernummern }).ToArray();


                var sqlVerein = @"SELECT v.Vereinsnummer, v.Bezeichnung FROM verein AS v;";

                var verein = connection.QueryFirst<VereinDTO>(sqlVerein);

                return new ReadResult() { A = adressen, P = pfleger, L = leistungen, V = verein };
            }
        }
    }
}
