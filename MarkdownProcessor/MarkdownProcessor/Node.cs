using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownProcessor
{
    public class ContentNode
    {
        public List<ContentNode> innerNodes;
        public string text;
        public ContentNode(List<ContentNode> innerNodes, string text)
        {
            this.innerNodes = innerNodes;
            this.text = text;
        }
    }
    public class Text : ContentNode { }
    public class Bold : ContentNode { }
    public class Italics : ContentNode { }
    public class Link : ContentNode
    {
        public string source;
    }
    public class Node
    {
        public List<ContentNode> content;
        public Node(List<ContentNode> content)
        {
            //fold the content nodes if they match
            this.content = new List<ContentNode>();
            ContentNode lastNode = content.FirstOrDefault();
            this.content.AddRange(content.Take(1));
            foreach (var node in content.Skip(1))
            {
                if (lastNode == null)
                {
                    this.content.Add(node);
                    lastNode = node;
                }
                else
                    if (lastNode.GetType() == node.GetType())
                    {
                        this.content.RemoveAt(this.content.Count - 1);
                        lastNode = new ContentNode(lastNode.innerNodes.Union(node.innerNodes).ToList(), lastNode.text + node.text);
                        this.content.Add(lastNode);
                    }
                    else
                    {
                        this.content.Add(node);
                        lastNode = node;
                    }
            }
        }
    }
}
