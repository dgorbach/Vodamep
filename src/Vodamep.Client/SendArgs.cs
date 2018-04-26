using PowerArgs;

namespace Vodamep.Client
{
    public class SendArgs
    {
        [ArgRequired, ArgPosition(1), ArgExistingFile]
        public string File { get; set; }
        
        [ArgRequired]
        public string Address { get; set; }
                
        public string User { get; set; }
                
        public string Password { get; set; }
    }
}
