using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Vodamep.Api
{
    public static class VodamepExtensions
    {
        public static IApplicationBuilder UseVodamep(this IApplicationBuilder app)
        {
            var handler = app.ApplicationServices.GetService<VodamepHandler>();

            var defaultHandler = new RequestDelegate(handler.HandleDefault);
            var vodamepHandler = new RequestDelegate(handler.HandlePut);

            return app.UseRouter(r =>
            {
                r.MapPut("{year:int}/{month:int}", vodamepHandler);

                r.MapGet("{year:int}/{month:int}", defaultHandler);
                r.MapGet("", vodamepHandler);
                r.DefaultHandler = new RouteHandler(defaultHandler);
            });
        }
    }
}
