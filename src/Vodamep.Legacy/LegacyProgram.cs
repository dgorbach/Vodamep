using PowerArgs;
using System;
using System.Linq;
using Vodamep.Legacy.Reader;

namespace Vodamep.Legacy
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [ArgDescription("(dml) Daten-Meldung-Legacy:")]
    public class LegacyProgram
    {
        [ArgActionMethod]
        [ArgDescription("Liest Daten aus einer vgkvdat.mdb.")]
        public void ReadMdb(ReadMdbArgs args)
        {
            var reader = new MdbReader(args.File);

            this.Read(reader, args);
        }

        [ArgActionMethod]
        [ArgDescription("Liest Daten aus dem connexia-Bestand.")]
        public void ReadConnexia(ReadConnexiaArgs args)
        {
            var vereine = ConnexiaReader.GetVereine(args.GetSqlServerCS(), args.Year);
            foreach (var verein in vereine)
            {
                var reader = new ConnexiaReader(args.GetSqlServerCS(), verein.Vereinsnummer);

                this.Read(reader, args);
            }
        }

        [ArgActionMethod]
        [ArgDescription("Liest Daten aus der Version 45 von TransDok.")]
        public void ReadTd45(ReadTd45Args args)
        {
            var reader = new Td45Reader(args.GetSqlServerCS());

            this.Read(reader, args);
        }

        private void Read(IReader reader, ReadBaseArgs args)
        {
            var year = args.Year;

            if (year == 0) year = DateTime.Today.AddMonths(-1).Year;


            int[] months;

            if (args.Month == 0)
            {
                if (year == DateTime.Today.Year)
                {
                    months = Enumerable.Range(1, DateTime.Today.AddMonths(-1).Month).ToArray();
                }
                else
                {
                    months = Enumerable.Range(1, 12).ToArray();
                }
            }
            else
            {
                months = new[] { args.Month };
            }

            foreach (var month in months)
            {
                var data = reader.Read(year, month);

                if (data == null || data.Equals(ReadResult.Empty))
                {
                    Console.WriteLine($"Keine Daten für {year}-{month}.");
                    continue;
                }

                var filename = new Writer().Write(data, args.Json);
                Console.WriteLine($"{filename} wurde erzeugt.");
            }
        }

    }
}
