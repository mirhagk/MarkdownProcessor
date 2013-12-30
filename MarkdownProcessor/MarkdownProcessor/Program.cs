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
This is a *test*";
        static void Main(string[] args)
        {
            MarkdownParser parser = new MarkdownParser();
            var result = (parser.Parse(test));
            foreach (var node in result)
            {
                Console.WriteLine(node);
            }
            Console.WriteLine("DONE. Press any key to quit");
            Console.ReadKey();
        }
    }
}
