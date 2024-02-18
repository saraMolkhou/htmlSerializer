﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace htmlSerializer
{
    public class selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public selector Parent { get; set; }
        public selector Child { get; set; }
        public selector()
        {
            TagName = "";
            Id = "";
            Classes = new List<string>();
            Parent = null;
            Child = null;
        }
        public static selector Create(string query)
        {
            var rootSelector = new selector();
            selector current = rootSelector;
            List<string> queryList = query.Split(' ').ToList();
            foreach (var line in queryList)
            {
                string[] segments = DivideString(line);
                foreach (var segment in segments)
                {
                    if (string.IsNullOrWhiteSpace(segment))
                        continue;

                    if (segment.StartsWith('#'))
                        current.Id = segment.Substring(1);
                    else if (segment.StartsWith('.'))
                        current.Classes.Add(segment.Substring(1));
                    else if (!string.IsNullOrEmpty(segment) && HtmlHelper.Instance.AllTagsList.Contains(segment))
                        current.TagName = segment;
                }

                if (!string.IsNullOrWhiteSpace(line) && line != queryList.Last())
                {
                    selector newSelector = new selector();
                    current.Child = newSelector;
                    newSelector.Parent = current;
                    current = newSelector;
                }
            }

            return rootSelector;
        }
        static string[] DivideString(string inputString)
        {
            string[] parts = inputString.Split('.', '#');

            string[] result = new string[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                if (i > 0)
                {
                    result[i] = inputString[inputString.IndexOf(parts[i]) - 1] + parts[i];
                }
                else
                {
                    result[i] = parts[i];
                }
            }

            return result;
        }
        public override bool Equals(object? obj)
        {
            if (obj is htmlElements)
            {
                htmlElements element = obj as htmlElements;

                if (element != null)
                {
                    bool tagNameMatches = string.IsNullOrEmpty(this.TagName) || this.TagName == element.Name;
                    bool idMatches = string.IsNullOrEmpty(this.Id) || this.Id == element.Id;
                    bool classesMatch = this.Classes.All(className => element.Classes.Contains(className));

                    return tagNameMatches && idMatches && classesMatch;
                }
            }
            return false;
        }
    }
}

