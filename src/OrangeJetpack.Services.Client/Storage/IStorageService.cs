using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace OrangeJetpack.Services.Client.Storage
{
    public interface IStorageService
    {
        Task<Uri> SaveFile(string containerName, IFormFile postedFile);
        Task<Uri[]> SaveImage(string containerName, IFormFile postedFile, ImageSettings imageSettings);
        Task DeleteFile(string containerName, string fileName);
    }
}
