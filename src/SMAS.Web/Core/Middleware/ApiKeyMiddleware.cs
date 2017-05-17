using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace SMAS.Web.Core.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly string _apiKey;
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next, string apiKey)
        {
            _next = next;
            _apiKey = apiKey;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Origin") && context.Request.Method.Equals("OPTIONS"))
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("");
                return;
            }

            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next.Invoke(context);
                return;
            }

            var apiKey = context.Request.Headers["X-ApiKey"];

            if (!apiKey.Any())
            {
                context.Response.StatusCode = 400; //Bad Request                
                await context.Response.WriteAsync("Api key is missing.");
                return;
            }

            if (apiKey != _apiKey)
            {
                context.Response.StatusCode = 400; //Bad Request                
                await context.Response.WriteAsync("Api key is invalid.");
                return;
            }

            await _next.Invoke(context);
        }
    }
}
