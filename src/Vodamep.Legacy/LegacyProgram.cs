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

            if (args.Year == 0) args.Year = DateTime.Today.AddMonths(-1).Year;


            int[] months;

            if (args.Month == 0)
            {
                if (args.Year == DateTime.Today.Year)
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
                var data = reader.Read(args.Year, args.Month);

                var filename = new Writer().Write(data, args.Json);

                Console.WriteLine($"{filename} wurde erzeugt.");
            }
        }
    }
}
