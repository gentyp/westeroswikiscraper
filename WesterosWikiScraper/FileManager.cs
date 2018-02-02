using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public static List<string> StoreUriListFromXml(string pfilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pfilePath);

            var uriList = new List<string>();
            XmlNodeList elemList = doc.GetElementsByTagName("loc");
            for(int i = 0; i < elemList.Count; i++)
            {
                uriList.Add(elemList[i].InnerText);
            }

            return uriList;
        }
    }
}
