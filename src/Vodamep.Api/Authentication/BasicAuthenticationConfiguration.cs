using System;

namespace Vodamep.Api.Authentication
{
    public class BasicAuthenticationConfiguration
    {
        public const string Mode_Disabled = "disabled";
        public const string Mode_Proxy = "Proxy";
        public const string Mode_UsernameEqualsPassword = "UsernameEqualsPassword";
        public const string Mode_UsernamePasswordUserGroup = "UsernamePasswordUserGroup";

        public string Mode { get; set; }

        public string Proxy { get; set; }

        public string Url { get; set; }
    }
}
