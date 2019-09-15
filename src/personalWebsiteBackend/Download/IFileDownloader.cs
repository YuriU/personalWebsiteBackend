using System;
using System.Threading.Tasks;

namespace personalWebsiteBackend.Download
{
    public interface IFileDownloader 
    {
        Task<string> DownloadBase64String(string bucketName, string fileName);
    }
}