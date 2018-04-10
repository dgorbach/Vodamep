using PowerArgs;
using System;

namespace Vodamep.Legacy
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            if (args.Length == 0)
                args = new[] { nameof(LegacyProgram.ReadMdb) };
#endif

            Args.InvokeAction<LegacyProgram>(args);
#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
