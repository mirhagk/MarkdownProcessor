using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownProcessor.Nodes
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
    public class Text : ContentNode {
        public Text(List<ContentNode> innerNodes, string text) : base(innerNodes, text) { }
    }
    public class Bold : ContentNode { 
    public Bold(List<ContentNode> innerNodes, string text) : base(innerNodes, text) { }
    }
    public class Italics : ContentNode { 
    public Italics(List<ContentNode> innerNodes, string text) : base(innerNodes, text) { }
    }
    public class Link : ContentNode
    {
        public Link(List<ContentNode> innerNodes, string text, string source)
            : base(innerNodes, text)
        {
            this.source = source;
        }
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
        public string DotDotDot(string text, int length)
        {
            return text.Length < length ? text : text.Substring(0, length - 3) + "...";
        }
        public override string ToString()
        {
            var text = string.Join("", content.Select((x) => x.text));
            return this.GetType().Name.Split('+').Last() + "\n\t" + DotDotDot(text, 50);
        }
    }
    public class ParagraphNode : Node
    {
        public ParagraphNode(List<ContentNode> content) : base(content) { }
    }
}
