using PowerArgs;

namespace Vodamep.Client
{
    public class PackFileArgs
    {
        [ArgRequired, ArgPosition(1), ArgExistingFile]
        public string File { get; set; }

        [ArgDescription("Save as JSON.")]
        public bool Json { get; set; }

        [DefaultValue(false)]
        public bool NoCompression { get; set; }
    }
}
