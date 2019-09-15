using System;
using System.Threading.Tasks;

namespace personalWebsiteBackend.Utils
{
    public interface IDownloadTracker 
    {
        Task TrackDownload(string fileName);
    }
}