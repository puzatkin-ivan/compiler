using System;
using System.IO;
using Compiler.Syntaxer;
using Compiler.Lexer;

namespace Compiler
{
    class Program
    {
        private static string _sourceCodeFileName = "../compiler-tests/main.lsd";

        public static void Main(string[] args)
        {
            GrammarStream stream = new GrammarStream(new StreamReader("../compiler-tests/syntax.test.lang"));
            Console.WriteLine(stream.GetGrammarText());
            Grammar grammar = new Grammar(stream.GetGrammarText());
            SyntaxAnalyzer analyzer = new SyntaxAnalyzer(stream);
            analyzer.Analyze();
        }

        private static void ReadSourceCode()
        {
            TextReader sourceCodeFile = new StreamReader(_sourceCodeFileName);
            Compiler.Lexer.Lexer lexer  = new Compiler.Lexer.Lexer(sourceCodeFile);
            var lexem = lexer.NextLexem();
            while (lexem != null) { Console.Write(lexem.Value); Console.Write(": "); Console.WriteLine(lexem.Type); lexem = lexer.NextLexem(); }
        }
    }
}
