using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownProcessor.Output
{
    class LaTeX : OutputFormat
    {
        protected override void Compile(System.IO.StreamWriter stream, IList<Nodes.Node> nodes)
        {
            foreach (var node in nodes)
            {
                var para = node as Nodes.ParagraphNode;
                if (para != null)
                    stream.WriteLine(string.Join("",para.content));
            }
        }
    }
}
