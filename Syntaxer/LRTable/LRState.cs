using System.Collections.Generic;

namespace Compiler.Syntaxer.Table
{
    public class LRState
    {
        public Dictionary<string, LRAction> Actions { get; private set; }

        public LRState()
        {
            Actions = new Dictionary<string, LRAction>();
        }
    }
}