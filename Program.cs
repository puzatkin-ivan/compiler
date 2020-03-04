using System;
using System.IO;
using Compiler.Lexer;

namespace Compiler
{
    class Program
    {
        private static string _sourceCodeFileName = "../compiler-tests/main.lsd";

        public static void Main(string[] args)
        {
            TextReader sourceCodeFile = new StreamReader(_sourceCodeFileName);
            Compiler.Lexer.Lexer lexer  = new Compiler.Lexer.Lexer(sourceCodeFile);
            var lexem = lexer.NextLexem();
            while (lexem != null) { Console.Write(lexem.Value); Console.Write(": "); Console.WriteLine(lexem.Type); lexem = lexer.NextLexem(); }

        }
    }
}
