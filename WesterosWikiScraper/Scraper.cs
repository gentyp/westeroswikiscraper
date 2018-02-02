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
    public class Scraper
    {
        public static void ScrapMultipleUri(List<string> pUriList)
        {
            for(int i = 0; i < pUriList.Count(); i++)
            {
                FileManager.WriteCsv("C:\\Users\\Pierre\\Documents\\WesterosWikiScraper\\WesterosWikiScraper\\database.csv", ScrapUrl(pUriList.ElementAt(i)));
            }
        }

        public static List<string> ScrapUrl(string pUri)
        {
            try
            {
                ScrapingBrowser Browser = new ScrapingBrowser();
                Browser.AllowAutoRedirect = true;
                Browser.AllowMetaRedirect = true;
                WebPage PageResult = Browser.NavigateToPage(new Uri(pUri));

                HtmlNode TitleNode = PageResult.Html.CssSelect("#firstHeading").First();
                string PageTitle = TitleNode.InnerText;

                var plainText = PageResult.Html.CssSelect("#mw-content-text");
                var nodes = plainText.First().SelectNodes("p").ToList();
                var rawHtml = new List<string>();

                rawHtml.Add(PageTitle);

                foreach (var row in nodes)
                {
                    rawHtml.Add(Strip(row.InnerText));
                }

                return rawHtml;
            }
            catch(Exception e)
            {
                var exceptionmessage = e.Message;
                return new List<string>();
            }
        }

        private static string Strip(string text)
        {
            return Regex.Replace(text, @"\[.*\]", string.Empty);
        }
    }
}
