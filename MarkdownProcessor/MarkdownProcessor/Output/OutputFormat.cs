using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownProcessor.Output
{
    public abstract class OutputFormat
    {
        protected abstract void Compile(System.IO.StreamWriter stream, List<Nodes.Node> nodes);
        private string GetStart()
        {
            string fileName = String.Format("Templates/{0}_start.txt", this.GetType().Name);
            return System.IO.File.ReadAllText(fileName);
        }
        private string GetEnd()
        {
            string fileName = String.Format("Templates/{0}_end.txt", this.GetType().Name);
            return System.IO.File.ReadAllText(fileName);
        }
        public void CompileToStream(System.IO.Stream stream, List<Nodes.Node> nodes)
        {
            var writer = new System.IO.StreamWriter(stream);
            writer.Write(GetStart());
            Compile(writer, nodes);
            writer.Write(GetEnd());
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
            CompileToStream(file.BaseStream, nodes);
            file.Close();
        }
    }
}
