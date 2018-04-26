using Bazinga.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Vodamep.Api.Authentication;
using Vodamep.Api.CmdQry;
using Vodamep.Api.Engines.FileSystem;
using Vodamep.Api.Engines.SqlServer;

namespace Vodamep.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this._configuration = configuration;
            this._loggerFactory = loggerFactory;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();

            this.ConfigureAuth(services);
            this.ConfigureEngine(services);

            services.AddTransient<VodamepHandler>();
            services.AddSingleton<DbUpdater>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            app.UseVodamep();
        }

        private void ConfigureEngine(IServiceCollection services)
        {
            var sqlEngineConfig = new SqlServerEngineConfiguration();
            this._configuration.Bind(nameof(SqlServerEngine), sqlEngineConfig);

            if (!string.IsNullOrEmpty(sqlEngineConfig.ConnectionString))
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using SqlServerEngine");

                services.AddTransient<Func<IEngine>>(sp => () => new SqlServerEngine(sqlEngineConfig, sp.GetService<DbUpdater>(), sp.GetService<ILogger<SqlServerEngine>>()));
                return;
            }

            var fileEngineConfig = new FileEngineConfiguration();
            this._configuration.Bind(nameof(FileEngine), fileEngineConfig);

            if (!string.IsNullOrEmpty(fileEngineConfig.Path))
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using FileEngine: '{path}'", fileEngineConfig.Path);
                services.AddTransient<Func<IEngine>>(sp => () => new FileEngine(fileEngineConfig, sp.GetService<ILogger<FileEngine>>()));
                return;
            }
            var msg = "No engine is configured";

            _loggerFactory.CreateLogger<Startup>().LogError(msg);
            throw new Exception(msg);
        }

        private void ConfigureAuth(IServiceCollection services)
        {
            var authConfig = new BasicAuthenticationConfiguration();
            this._configuration.Bind("BasicAuthentication", authConfig);

            if (authConfig.IsDisabled)
                return;

            Func<(string username, string password), Task<bool>> verifyDelegate = null;

            if (!string.IsNullOrEmpty(authConfig.Proxy))
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using ProxyAuthentication: {proxy}", authConfig.Proxy);
                verifyDelegate = new ProxyCredentialVerifier(new Uri(authConfig.Proxy)).Verify;
            }
            else if (authConfig.IsSimple)
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using UsernameEqualsPasswordCredentialVerifier");
                verifyDelegate = new UsernameEqualsPasswordCredentialVerifier().Verify;
            }

            if (verifyDelegate != null)
            {
                services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                    .AddBasicAuthentication(verifyDelegate);
                return;
            }


            var msg = "Authentication is not configured";

            _loggerFactory.CreateLogger<Startup>().LogError(msg);
            throw new Exception(msg);
        }
    }
}
