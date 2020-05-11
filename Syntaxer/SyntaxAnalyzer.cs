using System.IO;

using Compiler.Syntaxer;
using Compiler.Syntaxer.ClosureTable;
using Compiler.Syntaxer.Table;
using Compiler.Lexer;
using System;

namespace Compiler.Syntaxer
{
    public class SyntaxAnalyzer
    {
        private GrammarStream _grammarStream;
        private Grammar _grammar;
        private LRClosureTable _lrClosureTable;
        private LRTable _lrTable;

        public SyntaxAnalyzer(GrammarStream grammarStream)
        {
            _grammarStream = grammarStream;
            _grammar = new Grammar(_grammarStream.GetGrammarText());
            _lrClosureTable = new LRClosureTable(_grammar);
            _lrTable = new LRTable(_grammar, _lrClosureTable);
        }

        public void Analyze()
        {
            
        }
    }
}