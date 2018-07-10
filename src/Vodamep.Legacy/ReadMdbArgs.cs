using PowerArgs;

namespace Vodamep.Legacy
{

    public class ReadMdbArgs : ReadBaseArgs
    {
        [ArgExistingFile, DefaultValue(@"c:\verein\vgkvdat.mdb"), ArgPosition(1)]
        public string File { get; set; }
    }
}
