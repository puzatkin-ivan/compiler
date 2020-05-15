using System;
using System.Collections.Generic;
using compiler.Syntaxer.ParsingStackItem;
using Compiler.LexerAnalyzer;

namespace Compiler.Syntaxer.Table
{
    public class LRState
    {
        public Dictionary<string, LRAction> Actions { get; private set; }

        public LRState()
        {
            Actions = new Dictionary<string, LRAction>();
        }

        public LRAction GetActionByNode(Node node)
        {
            if (node.Index != -1)
            {
                return new LRAction("", node.Index);
            }

            if (node.Lexem != null)
            {
                if (TermRecognizer.ReservedWordTypeByString.ContainsKey(node.Lexem.Value)
                    || TermRecognizer.DelimeterTypeByString.ContainsKey(node.Lexem.Value))
                {
                    return Actions[node.Lexem.Value];
                }
                return Actions[node.Lexem.Type.ToString().ToLower()];
            }
            else
            {
                return Actions[node.NonTerminal.NonTerminal];
            }
        }
    }
}