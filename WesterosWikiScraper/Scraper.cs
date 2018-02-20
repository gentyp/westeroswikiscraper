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

        public static Content ScrapUrl(string pUri)
        {
            try
            {
                ScrapingBrowser Browser = new ScrapingBrowser();
                Browser.AllowAutoRedirect = true;
                Browser.AllowMetaRedirect = true;
                WebPage PageResult = Browser.NavigateToPage(new Uri(pUri));

                var res = new Content();
                res.Text = (PageContent(PageResult));
                res.Categories = (PageCategories(PageResult));
                res.StructuredContent = PageStructuredContent(PageResult);
                Console.WriteLine("Url: " + pUri + "\nFound " + res.Text.Count + " lines and " + res.Categories.Count + " categories");
                return res;
            }
            catch(Exception e)
            {
                var exceptionmessage = e.Message;
                return new Content();
            }
        }

        private static List<string> PageContent(WebPage pWebPage)
        {
            try
            {
                HtmlNode TitleNode = pWebPage.Html.CssSelect("#firstHeading").First();
                string PageTitle = TitleNode.InnerText;

                var plainText = pWebPage.Html.CssSelect("#mw-content-text");
                var nodes = plainText.First().SelectNodes("h2|h3|p").ToList();
                var rawHtml = new List<string>();

                rawHtml.Add(PageTitle);

                foreach (var row in nodes)
                {
                    switch (row.Name)
                    {
                        case "h2":
                            rawHtml.Add(Environment.NewLine + "section: " + Strip(row.InnerText) + Environment.NewLine);
                            break;
                        case "h3":
                            rawHtml.Add(Environment.NewLine + "subsection: " + Strip(row.InnerText) + Environment.NewLine);
                            break;
                        default:
                            rawHtml.Add(Strip(row.InnerText));
                            break;
                    }
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

        private static Dictionary<string, List<string>> PageStructuredContent(WebPage pWebPage)
        {
            try
            {
                var plainText = pWebPage.Html.CssSelect(".infobox");
                var nodes = plainText.First().SelectNodes("tr").ToList();
                var content = new Dictionary<string, List<string>>();

                int i = 0;
                foreach (var node in nodes)
                {
                    try
                    {
                        var rawHtml = new List<string>();

                        var text = new List<string>();
                        try
                        {
                            foreach(var subnode in node.SelectNodes("td"))
                            {
                                rawHtml.AddRange(Strip(subnode.InnerHtml.Split(new[] { "<br>" }, StringSplitOptions.None)));
                                
                            }
                            text = RawHtmlToCleanText(rawHtml);
                        }
                        catch
                        {
                            rawHtml.Add(Strip(node.SelectSingleNode("td").InnerText));
                        }

                        content.Add(Strip(node.SelectSingleNode("th").InnerText), text);
                    }
                    catch
                    {
                        //skip to next node
                    }
                   
                }
                
                return content;
            }
            catch
            {
                return new Dictionary<string, List<string>>();
            }

        }

        private static string Strip(string text)
        {
            return Regex.Replace(text, @" ?\[.*?\]", string.Empty);
        }

        private static string[] Strip(string[] text)
        {
            var striped = new List<string>() ;
            foreach(string str in text)
            {
                striped.Add(Strip(str));
            }

            return striped.ToArray();
        }

        private static List<string> RawHtmlToCleanText(List<string> pRawHtml)
        {
            var text = new List<string>();
            foreach (var row in pRawHtml)
            {
                text.Add(RawHtmlToCleanText(row));
            }

            return text;
        }

        private static string RawHtmlToCleanText(string pRawHtml)
        {
            return pRawHtml.ToHtmlNode().InnerText.CleanInnerText();
        }
    }
}
