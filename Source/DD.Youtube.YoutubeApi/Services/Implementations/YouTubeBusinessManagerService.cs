using DD.Youtube.YoutubeApi.Models;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DD.Youtube.YoutubeApi.Services.Implementations
{
    public class YouTubeBusinessManagerService : IYouTubeBusinessManagerService
    {
        private readonly IConfiguration configuration;
        private readonly IYouTubeManagerService youTubeManagerService;

        public YouTubeBusinessManagerService(
            IConfiguration configuration,
            IYouTubeManagerService youTubeManagerService)
        {
            this.configuration = configuration 
                ?? throw new ArgumentNullException(nameof(configuration));
            this.youTubeManagerService = youTubeManagerService 
                ?? throw new ArgumentNullException(nameof(youTubeManagerService));

            this.youTubeManagerService.OnProcessChanged += YouTubeManagerService_OnProcessChanged;
            this.youTubeManagerService.OnProcessCompleted += YouTubeManagerService_OnProcessCompleted;
        }

        private void YouTubeManagerService_OnProcessCompleted(object sender, Video e)
        {
            Console.WriteLine($"Video uploaded! {e.Id}");
        }

        private void YouTubeManagerService_OnProcessChanged(object sender, Google.Apis.Upload.IUploadProgress e)
        {
            Console.WriteLine($"{e.BytesSent} bytes sent...");
        }

        public async Task Search()
        {
            Console.WriteLine("Type search term");
            var term = Console.ReadLine();
            var results = await youTubeManagerService.Search(term);
            foreach (var item in results)
            {
                Console.WriteLine($"{item.Snippet.Title} ({item.Id.VideoId})");
            }
        }

        public async Task Upload(UploadRequestModel request)
        {
            var video = GetVideoFromRequest(request);
            Console.WriteLine("Requesting video upload");
            await youTubeManagerService.Upload(request.Path, video);
        }

        private Video GetVideoFromRequest(UploadRequestModel request)
        {
            return new Video()
            {
                Snippet = new VideoSnippet()
                {
                    Title = request.Title ?? configuration["default:title"],
                    Description = request.Description ?? configuration["default:description"],
                    CategoryId = request.CategoryId ?? configuration["default:categoryId"],
                    ChannelId = request.ChannelId ?? configuration["default:channelId"],
                    Tags = request.Tags ?? (string[])configuration.GetValue(typeof(string[]), "default:tags")
                }
            };
        }
    }
}
