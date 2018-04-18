using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Vodamep.Api
{
    public static class VodamepExtensions
    {
        public static IApplicationBuilder UseVodamep(this IApplicationBuilder app)
        {
            var handler = app.ApplicationServices.GetService<VodamepHandler>();

            return app.UseRouter(r =>
            {
                r.MapPut("{year:int}/{month:int}", handler.Handle);
            });
        }
    }
}
