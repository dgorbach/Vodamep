using System.Security.Principal;

namespace Vodamep.Api.CmdQry
{
    public class AuthContext
    {
        public AuthContext(IPrincipal principal)
        {
            this.Principal = principal;
        }
        public IPrincipal Principal { get; }
    }
}
