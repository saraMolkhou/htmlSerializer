using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace htmlSerializer
{
    public class TreeBuilder
    {
        public static htmlElements BuildTree(List<string> htmlLines)
        {
            var htmlHelper = HtmlHelper.Instance;
            htmlElements root = null;
            htmlElements current = root;
            htmlElements prev = null;
            var openTagsStack = new Stack<htmlElements>();

            foreach (string line in htmlLines)
            {
                if (line.StartsWith("!DOCTYPE"))
                    continue;

                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                    continue;

                var firstWord = line.Split(' ')[0];
                if (firstWord == "/html")
                    break;

                if (firstWord.StartsWith("/"))
                {
                    // Closing tag encountered
                    current = openTagsStack.Pop().Parent;
                }
                else
                {
                    var newElement = new htmlElements();
                    newElement.Name = firstWord;

                    var attributeList = new Regex("([^\\s]+)=\"(.+?)\"").Matches(line);
                    foreach (Match attribute in attributeList)
                    {
                        newElement.Attributes.Add(attribute.Value);
                        string attributeName = attribute.Groups[1].Value;
                        string attributeValue = attribute.Groups[2].Value;

                        if (attributeName.ToLower() == "class")
                        {
                            var listClass = attributeValue.Split(' ').ToList();
                            foreach (string listItem in listClass)
                            {
                                newElement.Classes.Add(listItem);
                            }
                        }
                        else if (attributeName.ToLower() == "id")
                        {
                            var id = attributeValue;
                            newElement.Id = id;
                        }
                    }

                    if (htmlHelper.AllTagsList.Contains(firstWord) && !htmlHelper.UnclosedTagsList.Contains(firstWord))
                    {
                        prev = current;
                        current = newElement;
                        if (root == null)
                        {
                            root = current;
                            root.Parent = null;
                            openTagsStack.Push(current);
                        }
                        else
                        {
                            // Push open tags onto the stack
                            if (openTagsStack.Count == 0 || openTagsStack.Peek() != current)
                            {
                                openTagsStack.Push(current);
                                prev.Children.Add(newElement);
                                newElement.Parent = prev;
                            }
                            else
                            {
                                current.Children.Add(newElement);
                                newElement.Parent = current;
                                current = newElement;
                            }
                        }

                    }
                    else
                    {
                        if (htmlHelper.UnclosedTagsList.Contains(firstWord))
                        {
                            // Self-closing tag
                            current.Children.Add(newElement);
                            newElement.Parent = current;
                        }
                        else
                        {
                            current.InnerHtml += line;
                        }
                    }

                }
            }
            return root;
        }
    }
}

