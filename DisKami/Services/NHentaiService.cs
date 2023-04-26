using Discord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Discord.Commands;
using Newtonsoft.Json;
using NHentaiSharp.Search;


namespace DisKami.Services
{

    public class NHentaiService
    {
        public static HttpClient HttpClient { get; } = new HttpClient();

        //get a douhjin with the specified tag from nhentai and convert it to type GalleryElement
        public static async Task<GalleryElement> getBook(string tag)
        {
            
            //get the json from the nhentai api
            string json = await HttpClient.GetStringAsync("https://nhentai.net/api/galleries/search?query=" + tag);

            //convert the json to a GalleryElement
            GalleryElement book = JsonConvert.DeserializeObject<GalleryElement>(json);
            return book;
        }
        
    }

}
