using Microsoft.AspNetCore.Builder;
using SMAS.Web.Core.Middleware;

namespace SMAS.Web.Core.Extensions
{
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ApiKeyMiddleware>();
            return builder;
        }
    }
}
