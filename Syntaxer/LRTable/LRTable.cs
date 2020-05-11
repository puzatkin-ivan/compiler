using System.Collections.Generic;

namespace compiler.Syntaxer.LRTable
{
    public class LRTable
    {
        public LRClosureTable ClosureTable { get; private set; }
        public List<int> States { get; private set; }

        public LRTable(LRClosureTable closureTable)
        {
            ClosureTable = closureTable;

            
        }
    }
}