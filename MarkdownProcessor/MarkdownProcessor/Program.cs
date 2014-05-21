using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownProcessor
{
    class Program
    {
        static string test = @"
Hello world
===
This is a *test*

[test]:  ho";
        static void Main(string[] args)
        {
            MarkdownParser parser = new MarkdownParser();
            var result = (parser.Parse(test));
            foreach (var node in result)
            {
                Console.WriteLine(node);
                Console.WriteLine("[{0}]", string.Join(",",
                    node.GetType().GetFields()
                    .Where(f=>f.Name!="content")
                    .Select(f => string.Format("{0} = {1}", f.Name, f.GetValue(node)))));
            }
            Console.WriteLine("Done parsing");
            Output.OutputFormat formatter = new Output.LaTeX();
            Console.WriteLine(formatter.CompileToString(result));
            Console.WriteLine("DONE. Press any key to quit");
            Console.ReadKey();
        }
    }
}
