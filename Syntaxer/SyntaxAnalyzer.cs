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
            Console.WriteLine("Success readed grammar");
            _lrClosureTable = new LRClosureTable(_grammar);
            Console.WriteLine("Success created closure table");
            _lrTable = new LRTable(_grammar, _lrClosureTable);
            Console.WriteLine("Success create lr-table");
        }

        public void Analyze()
        {
        }
    }
}