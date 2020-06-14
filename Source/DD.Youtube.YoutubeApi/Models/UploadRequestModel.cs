using System;
using System.Collections.Generic;
using System.Text;

namespace DD.Youtube.YoutubeApi.Models
{
    public class UploadRequestModel
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
        public string CategoryId { get; set; }
        public string ChannelId { get; set; }
    }
}
