using PowerArgs;
using System;
using Vodamep.Legacy.Reader;

namespace Vodamep.Legacy
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class LegacyProgram
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod]
        public void ReadMdb(ReadMdbArgs args)
        {
            
            var reader = new MdbReader(args.File);

            if (args.Year == 0) args.Year = DateTime.Today.AddMonths(-1).Year;
            if (args.Month == 0) args.Month = DateTime.Today.AddMonths(-1).Month;

            var data = reader.Read(args.Year, args.Month);

            var filename = new Writer().Write(data, args.Json);

            Console.WriteLine($"{filename} created");
        }
    }
}
