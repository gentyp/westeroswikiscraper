using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SampleScraperClient
{
    public class FileManager
    {
        public static void WriteCsv(string pFilePath, List<string> pToWrite)
        {
            using(var w = new StreamWriter(pFilePath, append:true))
            {
                try
                {
                    var csv = new StringBuilder();

                    csv.Append("#" + pToWrite.FirstOrDefault() + "\n");

                    pToWrite.RemoveAt(0);

                    foreach (var line in pToWrite)
                    {
                        csv.Append(line);
                    }

                    w.WriteLine(csv);
                    w.Flush();
                }
                catch(Exception e)
                {
                    var eMess = e.Message;
                    //do nothing if you can not write
                }

            }
           
        }

        public static void WriteInNewFile(List<string> pToWrite)
        {
            try
            {

                var filename = pToWrite.First();
                var path = "C:\\Users\\Pierre\\Source\\Repos\\westeroswikiscraper\\WesterosWikiScraper\\database\\" + filename + ".txt";
                pToWrite.RemoveAt(0);

                using (var w = File.AppendText(path))
                {
                    var csv = new StringBuilder();
                    int lines = 0;
                    foreach (var line in pToWrite)
                    {
                        csv.Append(line);
                        csv.Append("\n");
                        lines++;
                    }

                    Console.WriteLine("Writing " + lines + " lines in " + filename);

                    w.WriteLine(csv);
                    w.Flush();
                }
            }
            catch
            {
                //do nothing if you can not write
            }

        }

        public static void WriteInNewFile(List<List<string>> pToWrite)
        {
            try
            {

                var filename = pToWrite.First().First();
                var category = ChoseCategory(pToWrite.Last());
                if (!string.IsNullOrEmpty(category))
                    category += "\\";
                var path = "C:\\Users\\Pierre\\Source\\Repos\\westeroswikiscraper\\WesterosWikiScraper\\database\\" + category + filename + ".txt";
                pToWrite.First().RemoveAt(0);

                using (var w = File.AppendText(path))
                {
                    var csv = new StringBuilder();
                    int lines = 0;
                    foreach (var line in pToWrite.First())
                    {
                        csv.Append(line);
                        lines++;
                    }

                    Console.WriteLine("Writing " + lines + " lines in " + filename);

                    w.WriteLine(csv);
                    w.Flush();
                }
            }
            catch
            {
                //do nothing if you can not write
            }
        }

        public static List<string> StoreUriListFromXml(string pfilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pfilePath);

            var uriList = new List<string>();
            XmlNodeList elemList = doc.GetElementsByTagName("loc");
            int i;
            for(i = 0; i < elemList.Count; i++)
            {
                uriList.Add(elemList[i].InnerText);
                Console.WriteLine("Found Uri " + elemList[i].InnerText);
            }
            Console.WriteLine("Fond total of " + i + " Uris");
            Thread.Sleep(5000);
            return uriList;
        }

        private static string ChoseCategory(List<string> pCategories)
        {
            if (pCategories.Count == 0)
                return string.Empty;
            if (pCategories.Contains("Event"))
            {
                Console.WriteLine("Found a match for event");
                return "event";
            }
            else
                return string.Empty;
        }
    }
}
