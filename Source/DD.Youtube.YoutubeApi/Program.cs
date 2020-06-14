using DD.Youtube.YoutubeApi.Models;
using DD.Youtube.YoutubeApi.Services;
using DD.Youtube.YoutubeApi.Services.Implementations;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DD.Youtube.YoutubeApi
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await RunAsync(host.Services, args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("AppSettings.json", optional: false);
                    config.AddJsonFile("Secrets.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IYouTubeBusinessManagerService, YouTubeBusinessManagerService>();
                    services.AddScoped<IYouTubeManagerService, YouTubeManagerService>();
                });

        private static async Task RunAsync(IServiceProvider serviceProvider, string[] args)
        {
            try
            {
                var service = (IYouTubeBusinessManagerService)serviceProvider
                        .GetService(typeof(IYouTubeBusinessManagerService));

                UploadRequestModel inputData = args.Length switch
                {
                    0 => throw new ArgumentException("At least is necessary one parameter"),
                    1 => new UploadRequestModel() { Path = args.First() },
                    _ => Dodo.Console.ConsoleManager.ParseArguments<UploadRequestModel>(args)
                };

                await service.Upload(inputData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.Message);
            }
        }

    }
}
