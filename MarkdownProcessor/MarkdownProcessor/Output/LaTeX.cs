using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownProcessor.Output
{
    class LaTeX : OutputFormat
    {
        protected override void CompileToStream(System.IO.StreamWriter stream, List<Nodes.Node> nodes)
        {
            foreach (var node in nodes)
            {
            }
            throw new NotImplementedException();
        }
    }
}
