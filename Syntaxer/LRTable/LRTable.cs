using System;
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
            States = new List<LRState>();

            foreach (LRKernel kernel in ClosureTable.Kernels)
            {
                var state = new LRState();
                States.Add(state);

                foreach(string key in kernel.Keys)
                {
                    int nextStateIndex = kernel.Gotos[key];

                    string actionType = Grammar._terminals.Contains(key) ? "s" : "";
                    LRAction newAction = new LRAction(actionType, nextStateIndex);
                    if (!state.Actions.ContainsKey(key))
                    {
                        state.Actions.Add(key, newAction);
                    }
                    else
                    {
                        state.Actions[key] = newAction;
                    }
                    
                }

                foreach (LRKernelItem closure in kernel.Closure)
                {
                    if (closure.DotIndex == closure.Rule.Development.Length || closure.Rule.Development[0] == Epsilon)
                    {
                        foreach (string lookAhead in closure.LookAheads)
                        {
                            if (!state.Actions.ContainsKey(lookAhead))
                            {
                                state.Actions.Add(lookAhead, new LRAction("r", closure.Rule.Index));
                            }
                            else
                            {
                                state.Actions[lookAhead] = new LRAction("r", closure.Rule.Index);
                            }
                            
                        }
                    }
                }
            }
        }
    }
}