using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DisKami.Services
{
    public class NHentaiService
    {
       public async Task<NHentaiSharp.Search.GalleryElement> getBook(string t)
        {
            string[] tags = t.Split(" ");

            Random r = new Random();
            // We do a search with the tags
            var result = await NHentaiSharp.Core.SearchClient.SearchWithTagsAsync(tags);
            int page = r.Next(0, result.numPages) + 1; // Page count begin at 1
                                                       // We do a new search at a random page
            result = await NHentaiSharp.Core.SearchClient.SearchWithTagsAsync(tags, page);
            var doujinshi = result.elements[r.Next(0, result.elements.Length)]; // We get a random doujinshi
            
           return doujinshi;
        }
    }
}
