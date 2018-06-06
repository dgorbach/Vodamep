using PowerArgs;

namespace Vodamep.Legacy
{
    public abstract class ReadBaseArgs
    {        
        [ArgRange(1, 12)]
        public int Month { get; set; }

        public int Year { get; set; }

        [ArgDescription("Save as JSON.")]
        public bool Json { get; set; } = false;
    }
}
