using System;
using System.IO;
using Compiler.Syntaxer;
using compiler.Syntaxer.View;
using Compiler.Syntaxer.ClosureTable;
using Compiler.Syntaxer.Table;
using Compiler.LexerAnalyzer;
using compiler.Syntaxer.ParsingStackItem;
using compiler.Syntaxer.SyntaxTree;

namespace Compiler
{
    class Program
    {
        private static string _sourceCodeFileName = "../compiler-tests/main.test.lsd";
        private static string _outViewFileName = "./out/syntax_analyzer.md"; 

        public static void Main(string[] args)
        {
            
            TextReader sourceCodeFile = new StreamReader(_sourceCodeFileName);
            Lexer lexer  = new Lexer(sourceCodeFile);
            GrammarStream stream = new GrammarStream(new StreamReader("../compiler-tests/syntax.test.lang"));
            TextWriter writer = new StreamWriter(_outViewFileName);

            bool debug = args.Length == 1 && args[0] == "--debug";
            if (debug)
            {
                PrintGrammarView(stream, writer);
            }

            SyntaxAnalyzer analyzer = new SyntaxAnalyzer(stream);
            ASTree tree = analyzer.Analyze(lexer, debug, writer);

            if (debug)
            {
                writer.WriteLine("### Ast Tree");
                writer.WriteLine(tree.ToString());
                writer.Flush();
            }
        }

        private static void ReadSourceCode()
        {
            TextReader sourceCodeFile = new StreamReader(_sourceCodeFileName);
            Lexer lexer  = new Lexer(sourceCodeFile);
        }

        private static void PrintGrammarView(GrammarStream istream, TextWriter stream)
        {
            Grammar grammar = new Grammar(istream.GetGrammarText());
            LRClosureTable lRClosureTable = new LRClosureTable(grammar);
            var lrTable = new LRTable(grammar, lRClosureTable);
            var grammarView = new GrammarView(grammar);
            grammarView.View(stream);
            stream.Flush();
            var closureView = new ClosureTableView(lRClosureTable);
            closureView.View(stream);
            stream.Flush();
            var lrTableView = new LRTableView(lrTable);
            lrTableView.View(stream);
            stream.Flush();

        }
    }
}
