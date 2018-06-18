using PowerArgs;

namespace Vodamep.Client
{
    public class ValidateArgs
    {        
        [ArgRequired, ArgPosition(1)]
        public string File { get; set; }

        public bool IgnoreWarnings { get; set; } = false;
    }
}
