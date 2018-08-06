using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Newtonsoft.Json.Linq;

namespace Vodamep.Api.Authentication
{
    public class RestVerifier
    {
        private readonly RestVerifierClient client;

        public RestVerifier(string url)
        {
            client = new RestVerifierClient();
            client.BaseUrl = url;
        }

        public Task<bool> Verify((string username, string password) credentials)
        {
            Task<bool> result =  client.AuthenticateAsync(credentials.username, credentials.password, "HK");
            return result;
        }
    }
}
