using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownProcessor.Nodes
{
    public abstract class ContentNode
    {
        public IList<ContentNode> innerNodes;
        public string text;
        public ContentNode(IList<ContentNode> innerNodes, string text)
        {
            this.innerNodes = innerNodes ?? new List<ContentNode>();
            this.text = text;
        }
        public override string ToString()
        {
            if (text == null)
                return string.Join("", innerNodes.Select((x) => x.ToString()));
            return text;
        }
    }
    public class Text : ContentNode {
        public Text(IList<ContentNode> innerNodes, string text) : base(innerNodes, text) { }
    }
    public class Bold : ContentNode { 
    public Bold(IList<ContentNode> innerNodes, string text) : base(innerNodes, text) { }
    }
    public class Italics : ContentNode { 
    public Italics(IList<ContentNode> innerNodes, string text) : base(innerNodes, text) { }
    }
    public class Link : ContentNode
    {
        public Link(IList<ContentNode> innerNodes, string text, string source)
            : base(innerNodes, text)
        {
            this.source = source;
        }
        public string source;
    }
    public abstract class Node
    {
        public static List<ContentNode> FoldContentNodes(IList<ContentNode> content)
        {
            var result = content.Take(1).ToList();
            ContentNode lastNode = content.FirstOrDefault();
            foreach (var node in content.Skip(1))
            {
                if (lastNode.GetType() == node.GetType())
                {
                    result.RemoveAt(result.Count - 1);
                    var innerNodes = lastNode.innerNodes.Union(node.innerNodes).ToList();
                    var text = lastNode.text + node.text;
                    lastNode = lastNode.GetType().GetConstructor(new Type[] { typeof(IList<ContentNode>), typeof(string) }).Invoke(new object[] { innerNodes, text }) as ContentNode;
                }
                else
                    lastNode = node;
                result.Add(lastNode);
            }
            //Go through the resulting nodes and fold their nodes as well
            foreach (var node in result)
            {
                node.innerNodes = FoldContentNodes(node.innerNodes);
            }
            return result;
        }
        public List<ContentNode> content;
        public Node(IList<ContentNode> content)
        {
            //fold the content nodes if they match
            this.content = FoldContentNodes(content);
        }
        public string DotDotDot(string text, int length)
        {
            return text.Length < length ? text : text.Substring(0, length - 3) + "...";
        }
        public override string ToString()
        {
            var text = string.Join("", content.Select((x) => x.ToString()));
            return this.GetType().Name.Split('+').Last() + "\n\t" + DotDotDot(text, 50);
        }
    }
    public class ParagraphNode : Node
    {
        public ParagraphNode(IList<ContentNode> content) : base(content) { }
    }
    public class LineBreakNode : Node
    {
        public LineBreakNode() : base(new List<ContentNode>()) { }
    }
    public class HeadingNode : Node
    {
        public int Level;
        public HeadingNode(IList<ContentNode> content, int level)
            : base(content)
        {
            Level = level;
        }
    }
}
