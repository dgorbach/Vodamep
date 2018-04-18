using PowerArgs;
using System;
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
            if (args.Month == 0) args.Month = DateTime.Today.AddMonths(-1).Month;

            var data = reader.Read(args.Year, args.Month);

            var filename = new Writer().Write(data, args.Json);

            Console.WriteLine($"{filename} wurde erzeugt.");
        }
    }
}
