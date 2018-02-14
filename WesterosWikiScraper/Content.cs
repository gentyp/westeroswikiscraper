using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleScraperClient
{
    public class Content
    {
        public List<string> Text { get; set; }

        public List<string> Categories { get; set; }

        public Dictionary<string, List<string>> StructuredContent { get; set; }

        public Content()
        {
            this.Text = new List<string>();
            this.Categories = new List<string>();
        }

        public Content(List<string> pText, List<string> pCategories)
        {
            this.Text = pText;
            this.Categories = pCategories;
        }
    }
}
