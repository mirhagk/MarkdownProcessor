using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownProcessor.Output
{
    public abstract class OutputFormat
    {
        protected abstract void CompileToStream(System.IO.StreamWriter stream, List<Nodes.Node> nodes);
        public void CompileToStream(System.IO.Stream stream, List<Nodes.Node> nodes)
        {
            var writer = new System.IO.StreamWriter(stream);
            CompileToStream(writer, nodes);
        }
        public string CompileToString(List<Nodes.Node> nodes)
        {
            var stream = new System.IO.MemoryStream();
            var reader = new System.IO.StreamReader(stream);
            CompileToStream(stream, nodes);
            return reader.ReadToEnd();
        }
        public void CompileToFile(string fileName, List<Nodes.Node> nodes)
        {
            var file = new System.IO.StreamWriter(fileName);
            CompileToStream(file, nodes);
            file.Close();
        }
    }
}
