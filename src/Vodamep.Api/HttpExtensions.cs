using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Vodamep.Api
{
    public static class HttpExtensions
    {
        public static Task WriteJson<T>(this HttpResponse response, T obj)
        {
            response.ContentType = "application/json; charset=utf-8";
            var settings = new JsonSerializerSettings() {  Formatting = Formatting.Indented, DefaultValueHandling = DefaultValueHandling.Ignore };
                        
            return response.WriteAsync(JsonConvert.SerializeObject(obj, settings), encoding: System.Text.Encoding.UTF8);
        }
    }
}
