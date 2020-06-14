using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DD.Youtube.YoutubeApi.Services.Implementations
{
    public class YouTubeManagerService : IYouTubeManagerService
    {
        private const string ApplicationName = "dd-youtube-youtubeapi";
        private const string SearchPart = "snippet";
        private const string V = "youtube#video";
        private const string SecretsFilename = "Secrets.json";
        private readonly IConfiguration configuration;

        private enum ServiceType
        {
            ApiKey,
            OAuth,
        }

        public YouTubeManagerService(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public event EventHandler<IUploadProgress> OnProcessChanged;
        public event EventHandler<Video> OnProcessCompleted;

        public async Task<IEnumerable<SearchResult>> Search(string term)
        {
            var service = await GetService(ServiceType.ApiKey);
            var searchListRequest = service.Search.List(SearchPart);
            searchListRequest.Q = term;
            searchListRequest.MaxResults = 50;
            var results = await searchListRequest.ExecuteAsync();
            return results.Items.Where(k => k.Id.Kind == V);
        }

        public async Task Upload(string path, Video video)
        {
            var service = await GetService(ServiceType.OAuth);
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                var videosInsertRequest = service.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                videosInsertRequest.ProgressChanged += VideosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += VideosInsertRequest_ResponseReceived;
                await videosInsertRequest.UploadAsync();
            }
        }

        private async Task<YouTubeService> GetService(ServiceType type) =>
            type switch
            {
                ServiceType.ApiKey => GetApiKeyYoutubeService(),
                ServiceType.OAuth => await GetOAuthYoutubeService(),
                _ => throw new NotImplementedException()
            };

        private YouTubeService GetApiKeyYoutubeService()
        {
            var apiKey = configuration["api_key"];
            return  new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = ApplicationName,
            });
        }

        private async Task<YouTubeService> GetOAuthYoutubeService()
        {
            UserCredential credential;
            using (var stream = new FileStream(SecretsFilename, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets, new[] { YouTubeService.Scope.YoutubeUpload },
                    "user",
                    CancellationToken.None
                );
            }

            return new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        private void VideosInsertRequest_ResponseReceived(Video obj)
        {
            OnProcessCompleted?.Invoke(this, obj);
        }

        private void VideosInsertRequest_ProgressChanged(IUploadProgress obj)
        {
            OnProcessChanged?.Invoke(this, obj);
        }
    }
}
