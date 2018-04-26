using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Vodamep.Api.Authentication
{
    internal class ProxyCredentialVerifier
    {

        private readonly HttpClient _client;
        private IDictionary<string, bool> _verified = new SortedDictionary<string, bool>();
        private object _lock = new object();

        public ProxyCredentialVerifier(Uri address)
        {
            _client = new HttpClient()
            {
                BaseAddress = address,
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public Task<bool> Verify((string username, string password) credentials)
        {
            return Task.FromResult(Verify2(credentials));
        }

        private bool Verify2((string username, string password) credentials)
        {
            
            var user_pwd = $"{credentials.username}:{credentials.password}";

            if (_verified.ContainsKey(user_pwd))
                return _verified[user_pwd];

            lock (_lock)
            {
                if (_verified.ContainsKey(user_pwd))
                    return _verified[user_pwd];

                var byteArray = Encoding.ASCII.GetBytes(user_pwd);
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var r = _client.GetAsync("").Result;

                var result = r.IsSuccessStatusCode;

                _verified.Add(user_pwd, result);

                return result;
            }
        }
    }
}
