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
            //Crawls a URL that gives a list of characters, mapping the "href" attributes in the page and putting the links into xml
            //FileManager.WriteUriListInXmlFile(CrawlerSortOf.CrawlKinda("http://awoiaf.westeros.org/index.php/List_of_characters"));
            Scraper.ScrapMultipleUri(FileManager.StoreUriListFromXml("C:\\Users\\Pierre\\Source\\Repos\\westeroswikiscraper\\WesterosWikiScraper\\characters.xml"));
        }
    }
}
