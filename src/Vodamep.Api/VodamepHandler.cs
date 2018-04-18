using Bazinga.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Vodamep.Api.CmdQry;
using Vodamep.Hkpv;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;

namespace Vodamep.Api
{
    public class VodamepHandler
    {

        private readonly Func<IEngine> _engineFactory;
        private readonly ILogger<VodamepHandler> _logger;

        public VodamepHandler(Func<IEngine> engineFactory, ILogger<VodamepHandler> logger)
        {
            _engineFactory = engineFactory;
            _logger = logger;
        }
        private async Task RespondError(HttpContext context, string message)
        {
            _logger.LogWarning(message);

            var result = new SendResult() { IsValid = false, ErrorMessage = message };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteJson(result);
        }

        private async Task RespondSuccess(HttpContext context, string message)
        {
            var result = new SendResult() { IsValid = true, Message = message };

            context.Response.StatusCode = StatusCodes.Status200OK;

            await context.Response.WriteJson(result);
        }

        public async Task HandleDefault(HttpContext context)
        {
            await EnsureIsAuthenticated(context);

            if (!IsAuthenticated(context))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await context.Response.WriteAsync($"Verb =  {context.Request.Method.ToUpperInvariant()} - Path = {context.Request.Path} - Route values - {string.Join(", ", context.GetRouteData().Values)}");
        }

        public async Task HandlePut(HttpContext context)
        {
            if (context.Request.Method != HttpMethods.Put && context.Request.Method != HttpMethods.Post)
            {
                await HandleDefault(context);
                return;
            }

            await EnsureIsAuthenticated(context);

            if (!IsAuthenticated(context))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            int.TryParse((string)context.GetRouteValue("year"), out int year);
            int.TryParse((string)context.GetRouteValue("month"), out int month);


            if (year < 2000 || year > DateTime.Today.Year)
            {
                await RespondError(context, $"Ungültiges Jahr '{context.GetRouteValue("year")}'");
                return;
            }

            if (month < 1 || month > 12)
            {
                await RespondError(context, $"Ungültiger Monat '{context.GetRouteValue("month")}'");
                return;
            }

            HkpvReport report;
            try
            {
                report = new HkpvReportSerializer().Deserialize(context.Request.Body);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Deserialize failed.");
                report = null;
            }

            if (report == null)
            {
                await RespondError(context, $"Die Daten können nicht gelesen werden.");
                return;
            }

            var date = new DateTime(year, month, 1);
            if (report.FromD != date)
            {
                await RespondError(context, $"Ungültiger Zeitraum: {year}-{month}, entspricht nicht {report.From}.");
                return;
            }

            var validationResult = await new HkpvReportValidator().ValidateAsync(report);

            var msg = new HkpvReportValidationResultFormatter(ResultFormatterTemplate.Text).Format(report, validationResult);

            if (!validationResult.IsValid)
            {
                await RespondError(context, msg);
                return;
            }

            var saveCmd = new HkpvReportSaveCommand() { Report = report };

            _engineFactory().Execute(saveCmd);

            await RespondSuccess(context, msg);
        }

        private bool IsAuthenticated(HttpContext context) => context.User != null && !string.IsNullOrEmpty(context.User.Identity?.Name);
        private async Task EnsureIsAuthenticated(HttpContext context)
        {
            var authResult = await context.AuthenticateAsync(BasicAuthenticationDefaults.AuthenticationScheme);

            if (authResult.None)
                await context.ChallengeAsync(BasicAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
