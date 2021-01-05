using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using RestSharp;
using System.Threading;

namespace DisKami.Services
{
    public class PictureService
    {
        private readonly HttpClient _http;

        public PictureService(HttpClient http)
            => _http = http;

        public async Task<Stream> GetCatPictureAsync()
        {
            var resp = await _http.GetAsync("https://cataas.com/cat");
            return await resp.Content.ReadAsStreamAsync();
        }
        public async Task<string> GetGoogleResult(string query)
        {
            var client = new RestClient("https://bing-image-search1.p.rapidapi.com/images/search?q=%3C"+query+ "%3E&safesearch=off");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "bing-image-search1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "fb9545b61emsh146803d8e59810fp167c9bjsnfd7cb4df0b07");
            request.AddHeader("count", "1");
            IRestResponse response = await client.ExecuteAsync(request);
            string resp = response.Content;
            return resp;
        }
    }
}
