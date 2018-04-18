using PowerArgs;
using System;

namespace Vodamep.Client
{

    class Program
    {
        static int Main(string[] args)
        {            
            try
            {
                Args.InvokeAction<VodamepProgram>(args);
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(e.Message))
                    Console.WriteLine(e.Message);

                return -1;
            }

            return 0;
        }
    }

}
