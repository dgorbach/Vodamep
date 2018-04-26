using System.Threading.Tasks;

namespace Vodamep.Api.Authentication
{
    internal class UsernameEqualsPasswordCredentialVerifier
    {
        public Task<bool> Verify((string username, string password) credentials)
        {
            return Task.FromResult(credentials.username == credentials.password);
        }
    }
}
