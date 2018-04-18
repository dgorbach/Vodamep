using Bazinga.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Vodamep.Api.CmdQry;
using Vodamep.Api.Engines.FileSystem;
using Vodamep.Api.Engines.SqlServer;

namespace Vodamep.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var verifiery = new CredentialVerifier();

            services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasicAuthentication(verifiery.Verify);

            services.AddRouting();

            var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Vodamep");
            services.AddTransient<Func<IEngine>>(sp => () => new FileEngine(path, sp.GetService<ILogger<FileEngine>>()));

            services.AddTransient<VodamepHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            app.UseVodamep();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Hello World!");
            });

        }
    }
}
