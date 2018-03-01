using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SampleScraperClient
{
    public class CrawlerSortOf
    {
        public static List<string> CrawlKinda(string pUri)
        {
            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true;
            Browser.AllowMetaRedirect = true;
            WebPage PageResult = Browser.NavigateToPage(new Uri(pUri));

            var UriList = new List<string>();

            try
            {
                var mainPanel = PageResult.Html.CssSelect("#mw-content-text");
                var charList = mainPanel.First().SelectNodes("ul").ToList(); //lists of characters, 1 per letter in the alphabet
                foreach(var letter in charList)
                {
                    var nodeList = letter.SelectNodes("li").ToList();

                    foreach (var node in nodeList)
                    {
                        UriList.Add("http://awoiaf.westeros.org" + node.SelectNodes("a").First().Attributes["href"].Value);
                    }
                }
                

                return UriList;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error parsing URI : " + e.Message);
                return new List<string>();
            }
        }
    }
}
