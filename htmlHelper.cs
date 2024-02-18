using System;
using System.IO;
using System.Text.Json;

namespace htmlSerializer
{
    public class HtmlHelper
    {
        public string[] AllTagsList { get; set; }
        public string[] UnclosedTagsList { get; set; }

        private readonly static HtmlHelper instanc_ = new HtmlHelper();
        public static HtmlHelper Instance => instanc_;

        private HtmlHelper()
        {

            var allTagsJson = File.ReadAllText("HtmlTags.json");
            var unclosedTagsJson = File.ReadAllText("HtmlVoidTags.json");

            AllTagsList = JsonSerializer.Deserialize<string[]>(allTagsJson);
            UnclosedTagsList = JsonSerializer.Deserialize<string[]>(unclosedTagsJson);
        }
    }
}

