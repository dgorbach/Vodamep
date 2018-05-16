using Bazinga.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
        private BasicAuthenticationConfiguration _authConfig;

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

            services.AddTransient<VodamepHandler>(sp => new VodamepHandler(sp.GetService<Func<IEngine>>(), false, sp.GetService<ILogger<VodamepHandler>>()));
            services.AddSingleton<DbUpdater>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var useAuthentication = !string.Equals(_authConfig?.Mode, BasicAuthenticationConfiguration.Mode_Disabled, StringComparison.CurrentCultureIgnoreCase);
            if (useAuthentication)
            {
                app.UseAuthentication();
            }

            app.UseVodamep();
        }

        private void ConfigureEngine(IServiceCollection services)
        {
            var sqlEngineConfig = this._configuration.GetSection(nameof(SqlServerEngine)).Get<SqlServerEngineConfiguration>();

            if (!string.IsNullOrEmpty(sqlEngineConfig.ConnectionString))
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using SqlServerEngine");

                services.AddTransient<Func<IEngine>>(sp => () => new SqlServerEngine(sqlEngineConfig, sp.GetService<DbUpdater>(), sp.GetService<ILogger<SqlServerEngine>>()));
                return;
            }

            var fileEngineConfig = this._configuration.GetSection(nameof(FileEngine)).Get<FileEngineConfiguration>();

            if (string.IsNullOrEmpty(fileEngineConfig.Path))
            {
                fileEngineConfig.Path = ".";
            }

            _loggerFactory.CreateLogger<Startup>().LogInformation("Using FileEngine: '{path}'", fileEngineConfig.Path);
            services.AddTransient<Func<IEngine>>(sp => () => new FileEngine(fileEngineConfig, sp.GetService<ILogger<FileEngine>>()));
        }

        private void ConfigureAuth(IServiceCollection services)
        {
            _authConfig = _configuration.GetSection("BasicAuthentication").Get<BasicAuthenticationConfiguration>();

            if (string.Equals(_authConfig?.Mode, BasicAuthenticationConfiguration.Mode_Disabled, StringComparison.CurrentCultureIgnoreCase))
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Authentication is disabled");
                return;
            }

            if (string.Equals(_authConfig.Mode, BasicAuthenticationConfiguration.Mode_UsernameEqualsPassword, StringComparison.CurrentCultureIgnoreCase))
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using UsernameEqualsPasswordCredentialVerifier");

                services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                    .AddBasicAuthentication(new UsernameEqualsPasswordCredentialVerifier().Verify);

                return;
            }

            if (!string.IsNullOrEmpty(_authConfig.Proxy))
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using ProxyAuthentication: {proxy}", _authConfig.Proxy);

                services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                    .AddBasicAuthentication(new ProxyCredentialVerifier(new Uri(_authConfig.Proxy)).Verify);

                return;
            }

            var msg = "Authentication is not configured";

            _loggerFactory.CreateLogger<Startup>().LogError(msg);
            throw new Exception(msg);
        }
    }
}
