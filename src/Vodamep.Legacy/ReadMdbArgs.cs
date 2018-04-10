using PowerArgs;

namespace Vodamep.Legacy
{
    public class ReadMdbArgs
    {
        [ArgExistingFile, DefaultValue(@"c:\verein\vgkvdat.mdb")]
        public string File { get; set; }

        [ArgRange(1, 12)]        
        public int Month { get; set; }

        public int Year { get; set; }

        [ArgDescription("Save as JSON.")]
        public bool Json { get; set; } = false;
    }
}
