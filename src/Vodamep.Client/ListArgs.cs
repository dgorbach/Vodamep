using PowerArgs;

namespace Vodamep.Client
{

    public enum ListSources
    {
        Insurances,
        Religions,
        CountryCodes,
        Postcode_City,
        Qualifications,
    }
    public class ListArgs
    {  
        [ArgRequired, ArgPosition(1)]
        public ListSources Source { get; set; }

    }

}
