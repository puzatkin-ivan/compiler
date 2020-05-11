using System.Collections.Generic;
using Compiler.Syntaxer.ClosureTable;

namespace Compiler.Syntaxer.Table
{
    public class LRTable
    {
        private const string Epsilon = "\'\'";
        public Grammar Grammar { get; private set; }
        public LRClosureTable ClosureTable { get; private set; }
        public List<LRState> States { get; private set; }

        public LRTable(Grammar grammar, LRClosureTable closureTable)
        {
            Grammar = grammar;
            ClosureTable = closureTable;

            foreach (LRKernel kernel in ClosureTable.Kernels)
            {
                var state = new LRState();

                foreach(string key in kernel.Keys)
                {
                    int nextStateIndex = kernel.Gotos[key];

                    string actionType = Grammar._terminals.Contains(key) ? "s" : "";
                    LRAction newAction = new LRAction(actionType, nextStateIndex);
                    state.Actions.Add(key, newAction);
                }

                foreach (LRKernelItem closure in kernel.Closure)
                {
                    if (closure.DotIndex == closure.Rule.Development.Length || closure.Rule.Development[0] != Epsilon)
                    {
                        foreach (string lookAhead in closure.LookAheads)
                        {
                            state.Actions.Add(lookAhead, new LRAction("r", closure.Rule.Index));
                        }
                    }
                }
            }
        }
    }
}