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
                if (File.Exists(path))
                    File.Delete(path);

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

        public static void WriteInNewFile(Content pToWrite)
        {
            try
            {

                var filename = pToWrite.Text.First();
                var path = "C:\\Users\\Pierre\\Source\\Repos\\westeroswikiscraper\\WesterosWikiScraper\\database\\" + filename + ".txt";
                pToWrite.Text.RemoveAt(0);
                if (File.Exists(path))
                    File.Delete(path);

                using (var w = File.AppendText(path))
                {
                    var csv = new StringBuilder();

                    csv.Append("Categories: ");
                    foreach (var cat in pToWrite.Categories)
                    {
                        csv.Append(cat);
                        csv.Append(", ");
                    }

                    var index = csv.ToString().LastIndexOf(",");

                    csv.Remove(index, 1);

                    csv.Append(Environment.NewLine);

                    foreach (var item in pToWrite.StructuredContent)
                    {
                        
                        foreach (var line in item.Value)
                        {
                            csv.Append(Environment.NewLine);
                            csv.Append(item.Key + ":" + line);
                        }
                    }
                    csv.Append(Environment.NewLine + Environment.NewLine + "section: Presentation" + Environment.NewLine);
                    int lines = 0;
                    foreach (var line in pToWrite.Text)
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
            //Thread.Sleep(5000);
            return uriList;
        }
    }
}
