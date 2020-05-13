using System;
using System.IO;
using Compiler.Syntaxer;
using Compiler.Lexer;
using compiler.Syntaxer.View;
using Compiler.Syntaxer.ClosureTable;

namespace Compiler
{
    class Program
    {
        private static string _sourceCodeFileName = "../compiler-tests/main.lsd";
        private static string _outViewFileName = "./out/syntax_analyzer.md"; 

        public static void Main(string[] args)
        {
            Console.WriteLine("Start compiler");
            GrammarStream stream = new GrammarStream(new StreamReader("../compiler-tests/syntax.test.lang"));
            
            SyntaxAnalyzer analyzer = new SyntaxAnalyzer(stream);
            analyzer.Analyze();
            // PrintGrammarView(stream);
        }

        private static void ReadSourceCode()
        {
            TextReader sourceCodeFile = new StreamReader(_sourceCodeFileName);
            Compiler.Lexer.Lexer lexer  = new Compiler.Lexer.Lexer(sourceCodeFile);
            var lexem = lexer.NextLexem();
            while (lexem != null) { Console.Write(lexem.Value); Console.Write(": "); Console.WriteLine(lexem.Type); lexem = lexer.NextLexem(); }
        }

        private static void PrintGrammarView(GrammarStream istream)
        {
            Grammar grammar = new Grammar(istream.GetGrammarText());
            LRClosureTable lRClosureTable = new LRClosureTable(grammar);
            var stream = new StreamWriter(_outViewFileName);
            var grammarView = new GrammarView(grammar);
            grammarView.View(stream);
            var closureView = new ClosureTableView(lRClosureTable);
            stream.Flush();
            closureView.View(stream);
            stream.Flush();
        }
    }
}
