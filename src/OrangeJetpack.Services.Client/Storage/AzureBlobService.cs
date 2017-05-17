using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrangeJetpack.Services.Client.Storage
{
    public class AzureBlobService : IStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of an AzureBlogService, by default uses a connection string
        /// called "StorageConnection" defined in web.config or app.config.
        /// </summary>
        public AzureBlobService(string projectKey, string projectToken, string connectionString)
        {
            _httpClient = HttpClientFactory.Create(projectKey, projectToken);
            _connectionString = connectionString;
        }

        /// <summary>
        /// Saves a file to Azure blog storage using the Orange Jetpack services REST API.
        /// </summary>
        /// <param name="containerName">The name of the Azure blog storage container to save the image to.</param>
        /// <param name="postedFile">The file to save.</param>
        /// <returns>The URL of the newly saved file.</returns>
        public async Task<Uri> SaveFile(string containerName, IFormFile postedFile)
        {
            var fileData = await GetFileData(postedFile);
            var fileName = Path.GetFileName(postedFile.FileName);

            using (var content = new MultipartFormDataContent("Upload----" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)))
            {
                content.Add(new ByteArrayContent(fileData), "file", fileName);
                var query = new Dictionary<string, string>
                {
                    {"StorageConnection", _connectionString},
                    {"ContainerName", containerName},
                    {"FileName",fileName},
                    {"ContentType", postedFile.ContentType}
                };

                var uri = $"{_httpClient.BaseAddress}/storage/file";
                uri = QueryHelpers.AddQueryString(uri, query);

                using (var message = await _httpClient.PostAsync(new Uri(uri), content))
                {
                    var response = await message.Content.ReadAsStringAsync();
                    if (message.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(response);
                    }

                    return JsonConvert.DeserializeObject<Uri>(response);
                }
            }
        }

        /// <summary>
        /// Saves an image to Azure blog storage using the Orange Jetpack services REST API.
        /// </summary>
        /// <param name="containerName">The name of the Azure blog storage container to save the image to.</param>
        /// <param name="postedFile">The image file to save.</param>
        /// <param name="imageSettings">The image resize settings to use.</param>
        /// <returns>A collection of URLs for the saved image(s).</returns>
        public async Task<Uri[]> SaveImage(string containerName, IFormFile postedFile, ImageSettings imageSettings)
        {
            var fileData = await GetFileData(postedFile);
            var fileName = Path.GetFileName(postedFile.FileName);

            using (
                var content =
                    new MultipartFormDataContent("Upload----" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)))
            {
                content.Add(new ByteArrayContent(fileData), "image", fileName);
                var uri = $"{_httpClient.BaseAddress}/storage/image";

                var query = new Dictionary<string, string>
                {
                    {"StorageConnection", _connectionString},
                    { "ContainerName", containerName},
                    {"FileName", fileName},
                    {"ContentType", postedFile.ContentType},
                    {"Widths", string.Join("|", imageSettings.Widths)},
                    {"ForceSquare", imageSettings.ForceSquare.ToString()},
                    {"BackgroundColor", ColorTranslator.ToHtml(Color.FromArgb(imageSettings.BackgroundColor.ToArgb()))},
                    {"Watermark", imageSettings.Watermark}
                };
                uri = QueryHelpers.AddQueryString(uri, query);

                using (var message = await _httpClient.PostAsync(new Uri(uri), content))
                {
                    var response = await message.Content.ReadAsStringAsync();
                    if (message.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(response);
                    }

                    return JsonConvert.DeserializeObject<Uri[]>(response);
                }
            }
        }

        public async Task DeleteFile(string containerName, string fileName)
        {
            var query = new Dictionary<string, string>
            {
                {"StorageConnection", _connectionString },
                { "ContainerName", containerName},
                { "FileName", fileName}
            };

            var uri = $"{_httpClient.BaseAddress}/storage/file";
            uri = QueryHelpers.AddQueryString(uri, query);

            using (var message = await _httpClient.DeleteAsync(uri))
            {
                var response = await message.Content.ReadAsStringAsync();
                if (message.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response);
                }
            }
        }

        private static async Task<byte[]> GetFileData(IFormFile postedFile)
        {
            using (var stream = new MemoryStream())
            {
                postedFile.OpenReadStream().Position = 0;
                await postedFile.OpenReadStream().CopyToAsync(stream);
                stream.Position = 0;
                return stream.ToArray();
            }
        }
    }
}
