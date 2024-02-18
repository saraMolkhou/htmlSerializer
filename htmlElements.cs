using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace htmlSerializer
{
    public class htmlElements
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public htmlElements Parent { get; set; }
        public List<htmlElements> Children { get; set; }

        public htmlElements()
        {
            Name = "";
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<htmlElements>(); 
            InnerHtml = "";
        }
        public IEnumerable<htmlElements> Descendants()
        {
            Queue<htmlElements> queue = new Queue<htmlElements>();
            queue.Enqueue(this);
            htmlElements currentElement;
            while (queue.Count > 0)
            {
                currentElement = queue.Dequeue();
                yield return currentElement;

                if (currentElement != null)
                {
                    if (currentElement.Children == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (currentElement.Children.Count > 0)
                        {
                            foreach (var child in currentElement.Children)
                            {
                                queue.Enqueue(child);
                            }
                        }
                    }
                }
            }
        }
        public IEnumerable<htmlElements> Ancestors(htmlElements element)
        {
            htmlElements current = element;
            while (current.Parent != null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }
    }
}
