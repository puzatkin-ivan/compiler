using System.IO;
using Compiler.Syntaxer.ClosureTable;

namespace compiler.Syntaxer.View
{
    public class ClosureTableView
    {
        private LRClosureTable _closureTable;

        public ClosureTableView(LRClosureTable closureTable)
        {
            _closureTable = closureTable;
        }

        public void View(TextWriter writer)
        {
            writer.WriteLine("Closure Table");

            writer.WriteLine("Goto | Kernel | State | Closure");
            writer.WriteLine("| ----------- | -----------| ----------- | -----------|");

            foreach (LRKernel kernel in _closureTable.Kernels)
            {
                for (int index = 0; index < kernel.Keys.Count; ++index)
                {
                    writer.Write("| goto(" + kernel.Index + ", " + kernel.Keys[index] + ") |");
                    writer.Write("| | |");
                }
                writer.WriteLine();
            }
        }
    }
}