using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SMAS.Web.Core.Middleware;

namespace SMAS.Web.Core.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder, string apiKey)
        {
            builder.UseMiddleware<ApiKeyMiddleware>(apiKey);
            return builder;
        }

        public static IApplicationBuilder UseTokenProviderMiddleware(this IApplicationBuilder builder,
            TokenProviderOptions option)
        {
            builder.UseMiddleware<TokenProviderMiddleware>(Microsoft.Extensions.Options.Options.Create(option));
            return builder;
        }
    }
}
