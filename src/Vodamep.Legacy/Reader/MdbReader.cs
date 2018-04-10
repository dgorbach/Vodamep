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
WHERE(l.Datum Between @from And @to);";

                var leistungen = connection.Query<LeistungDTO>(sqlLeistungen, new { from = from, to = to }).ToArray();

                var adressnummern = leistungen.Where(x => x.Leistung < 20).Select(x => x.Adressnummer).Distinct().ToArray();

                var sqlAdressen = @"SELECT a.Adressnummer, a.Name_1, a.Name_2, a.Adresse, a.Land, a.Postleitzahl, a.Geburtsdatum, a.Staatsbuergerschaft, a.Versicherung, a.Versicherungsnummer, o.Ort
FROM  Adressen AS a LEFT JOIN tb_orte AS o  ON o.Postleitzahl = a.Postleitzahl 
where a.Adressnummer in @ids;";

                var adressen = connection.Query<AdresseDTO>(sqlAdressen, new { ids = adressnummern }).ToArray();

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
