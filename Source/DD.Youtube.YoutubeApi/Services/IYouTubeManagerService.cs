using Google.Apis.Upload;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DD.Youtube.YoutubeApi.Services
{
    public interface IYouTubeManagerService
    {
        Task<IEnumerable<SearchResult>> Search(string term);
        Task Upload(string path, Video video);

        event EventHandler<IUploadProgress> OnProcessChanged;
        event EventHandler<Video> OnProcessCompleted;
    }
}
