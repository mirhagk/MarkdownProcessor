using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownProcessor
{
    partial class MarkdownParser
    {
        public List<T> FlattenList<T>(IEnumerable<IEnumerable<T>> listOfList)
        {
            return Flatten(listOfList).ToList();
        }
        public IEnumerable<T> Flatten<T>(IEnumerable<IEnumerable<T>> listOfList)
        {
            return listOfList.SelectMany((x) => x);
        }
        public string Flatten(IEnumerable<string> text)
        {
            return string.Join("", text);
        }
    }
}
