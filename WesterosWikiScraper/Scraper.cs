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
                //FileManager.WriteCsv("C:\\Users\\Pierre\\Documents\\WesterosWikiScraper\\WesterosWikiScraper\\database.csv", ScrapUrl(pUriList.ElementAt(i)));
                FileManager.WriteInNewFile(ScrapUrl(pUriList.ElementAt(i)));
            }
        }

        public static List<List<string>> ScrapUrl(string pUri)
        {
            try
            {
                ScrapingBrowser Browser = new ScrapingBrowser();
                Browser.AllowAutoRedirect = true;
                Browser.AllowMetaRedirect = true;
                WebPage PageResult = Browser.NavigateToPage(new Uri(pUri));

                var res = new List<List<string>> ();
                res.Add(PageContent(PageResult));
                res.Add(PageCategories(PageResult));
                Console.WriteLine("Url: " + pUri + "\nFound " + res.ElementAt(0).Count + " lines and " + res.ElementAt(1).Count + " categories");
                return res;
            }
            catch(Exception e)
            {
                var exceptionmessage = e.Message;
                return new List<List<string>>();
            }
        }

        private static List<string> PageContent(WebPage pWebPage)
        {
            try
            {
                HtmlNode TitleNode = pWebPage.Html.CssSelect("#firstHeading").First();
                string PageTitle = TitleNode.InnerText;

                var plainText = pWebPage.Html.CssSelect("#mw-content-text");
                var nodes = plainText.First().SelectNodes("p").ToList();
                var rawHtml = new List<string>();

                rawHtml.Add(PageTitle);

                foreach (var row in nodes)
                {
                    rawHtml.Add(Strip(row.InnerText));
                }

                return rawHtml;
            }
            catch
            {
                return new List<string>();
            }

        }

        private static List<string> PageCategories(WebPage pWebPage)
        {
            try
            {
                var plainText = pWebPage.Html.CssSelect("#mw-normal-catlinks");
                var nodes = plainText.First().SelectSingleNode("ul").SelectNodes("li").ToList();
                var rawHtml = new List<string>();

                int i = 0;
                foreach (var row in nodes)
                {
                    rawHtml.Add(Strip(row.InnerText));
                    i++;
                }
                Console.WriteLine("Found " + i + " categories");
                return rawHtml;
            }
            catch
            {
                return new List<string>();
            }

        }

        private static string Strip(string text)
        {
            return Regex.Replace(text, @"\[.*\]", string.Empty);
        }
    }
}
