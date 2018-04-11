using PowerArgs;

namespace Vodamep.Client
{

    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<VodamepProgram>(args);
        }
    }

}
