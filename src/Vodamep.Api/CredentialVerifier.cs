using System.Threading.Tasks;

namespace Vodamep.Api
{
    internal class CredentialVerifier
    {
        public Task<bool> Verify((string username, string password) credentials)
        {
            return Task.FromResult(credentials.username == credentials.password);
        }
    }
}
