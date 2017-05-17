using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OrangeJetpack.Services.Client
{
    internal static class HttpClientFactory
    {
        //private const string BaseAddress = "https://services.orangejetpack.com/api/";
        private const string BaseAddress = "https://orangejetpackservices.azurewebsites.net/api/";
        //private const string BaseAddress = "http://localhost:51941/api/";

        public static HttpClient Create(string projectKey, string projectToken)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(BaseAddress) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = GetBasicAuthenticationHeader(projectKey, projectToken);

            return httpClient;
        }

        private static AuthenticationHeaderValue GetBasicAuthenticationHeader(string key, string token)
        {
            var parameters = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", key, token));
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(parameters));
        }
    }
}
