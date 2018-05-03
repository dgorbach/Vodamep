using System;

namespace Vodamep.Api.Authentication
{
    public class BasicAuthenticationConfiguration
    {
        public const string Mode_Disabled = "disabled";
        public const string Mode_Proxy = "Proxy";
        public const string Mode_UsernameEqualsPassword = "UsernameEqualsPassword";

        public string Mode { get; set; }

        public string Proxy { get; set; }
    }
}
