using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace html_serializer
{
    using System.Text.Json;

    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }

        private HtmlHelper()
        {
            var allTagsJson = File.ReadAllText("JSON Files/HtmlTags.json");
            AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);

            var selfClosingJson = File.ReadAllText("JSON Files/HtmlVoidTags.json");
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingJson);
        }
    }
}
