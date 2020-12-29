using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YouTubeSearch;

namespace DisKami.Services
{
    public class YoutubeService
    {
        VideoSearch videos = new VideoSearch();
        public async Task<string> GetVideo(string query)
        {
            var items = await videos.GetVideos(query, 1);
            return items[0].getUrl();
        }
    }
}
