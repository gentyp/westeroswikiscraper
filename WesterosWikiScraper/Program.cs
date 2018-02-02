using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleScraperClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Scraper.ScrapMultipleUri(FileManager.StoreUriListFromXml("C:\\Users\\Pierre\\Source\\Repos\\westeroswikiscraper\\WesterosWikiScraper\\sitemap.xml"));
        }
    }
}
