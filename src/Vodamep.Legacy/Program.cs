using PowerArgs;
using System;

namespace Vodamep.Legacy
{
    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<LegacyProgram>(args);
#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
