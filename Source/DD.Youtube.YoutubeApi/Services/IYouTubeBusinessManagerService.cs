using DD.Youtube.YoutubeApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DD.Youtube.YoutubeApi.Services
{
    public interface IYouTubeBusinessManagerService
    {
        Task Search();
        Task Upload(UploadRequestModel request);
    }
}
