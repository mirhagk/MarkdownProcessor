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
            Console.WriteLine(parser.Parse(test));
            Console.ReadKey();
        }
    }
}
