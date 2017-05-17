using System;

namespace SMAS.Core.Extensions
{
    public static class UriExtensions
    {
        public static Uri FromCdn(this Uri uri)
        {
            var uriBuilder = new UriBuilder(uri)
            {
                Host = "wh.azureedge.net",
                Scheme = "https",
                Port = -1
            };

            return uriBuilder.Uri;
        }
    }
}
