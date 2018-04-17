using PowerArgs;

namespace Vodamep.Client
{
    public class PackRandomArgs
    {
        [ArgRange(1, 12)]
        public int Month { get; set; }

        public int Year { get; set; }

        [DefaultValue(100), ArgRange(0, 1000)]
        public int Persons { get; set; }

        [DefaultValue(5), ArgRange(0, 100)]
        public int Staffs { get; set; }

        [DefaultValue(true)]
        public bool AddActivities { get; set; } = true;

        [ArgDescription("Save as JSON.")]
        public bool Json { get; set; } = false;

        [DefaultValue(false)]
        public bool NoCompression { get; set; }
    }

}
