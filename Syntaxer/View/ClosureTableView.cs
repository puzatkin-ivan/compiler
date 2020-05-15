using System.Collections.Generic;
using System.IO;
using System.Linq;
using Compiler.Syntaxer.ClosureTable;

namespace compiler.Syntaxer.View
{
    public class ClosureTableView
    {
        private const string Epsilon = "\'\'";
        private LRClosureTable _closureTable;

        public ClosureTableView(LRClosureTable closureTable)
        {
            _closureTable = closureTable;
        }

        public void View(TextWriter writer)
        {
            writer.WriteLine("### Closure Table");

            writer.WriteLine("Goto | Kernel | State | Closure");
            writer.WriteLine("| ----------- | -----------| ----------- | -----------|");

            writer.Write("| &nbsp; | ");
            writer.Write(FormatItems(_closureTable.Kernels.First().Items) + " | ");
            writer.Write(0 + " | ");
            writer.WriteLine(FormatItems(_closureTable.Kernels.First().Closure) + " |");

            HashSet<int> done = new HashSet<int>() { 0 };
            foreach (LRKernel kernel in _closureTable.Kernels)
            {
                foreach (string key in kernel.Keys)
                {
                    LRKernel targetKernel = _closureTable.Kernels.ToList()[kernel.Gotos[key]];
                    writer.Write("| goto(" + kernel.Index + ", " + key + ") | ");
                    writer.Write(FormatItems(targetKernel.Items) + " | ");
                    writer.Write(targetKernel.Index + " | ");

                    bool isProcessed = done.ToList().Contains(targetKernel.Index);
                    string formatClosure = isProcessed ? "&nbsp;" : FormatItems(targetKernel.Closure );
                    writer.WriteLine(formatClosure + " |");

                    done.Add(targetKernel.Index);
                }
            }
        }

        private string FormatItems(List<LRKernelItem> items)
        {
            string result = "{ ";

            foreach (LRKernelItem item in items)
            {
                bool itemIsFinal = item.DotIndex == item.Rule.Development.Length || item.Rule.Development[0] == Epsilon;
                result += itemIsFinal ? "**" + item.ToString() + "**" : item.ToString();
                result += "; ";
            }
            result += " }";

            return result;
        }
    }
}