using System;
using System.IO;
using Compiler.Syntaxer;
using compiler.Syntaxer.View;
using Compiler.Syntaxer.ClosureTable;
using Compiler.Syntaxer.Table;
using Compiler.LexerAnalyzer;
using compiler.Syntaxer.ParsingStackItem;
using compiler.Syntaxer.SyntaxTree;
using compiler.ILGenerator;
using System.Collections.Generic;

namespace Compiler
{
    class Program
    {
        private static string _sourceCodeFileName = "./source/main.test.lsd";
        private static string _outViewFileName = "./out/syntax_analyzer.md"; 

        public static void Main(string[] args)
        {
            bool debug = args.Length == 1 && args[0] == "--debug";

            TextReader sourceCodeFile = new StreamReader(_sourceCodeFileName);
            Lexer lexer  = new Lexer(sourceCodeFile);

            GrammarStream stream = new GrammarStream(new StreamReader("./source/syntax.lang"));
            TextWriter writer = new StreamWriter(_outViewFileName);
            if (debug)
            {
                PrintGrammarView(stream, writer);
            }
            SyntaxAnalyzer analyzer = new SyntaxAnalyzer(stream);
            var tree = analyzer.Analyze(lexer, debug, writer);


            return;
            /* if (debug)
            {
                writer.WriteLine("### Ast Tree");

                writer.WriteLine(tree.ToString());
                writer.Flush();
            }

            MSILGenerator generator = new MSILGenerator();
            generator.Generate(new StreamWriter("./out/lsdlang.il"), tree); */

        }

        private static void ReadSourceCode()
        {
            TextReader sourceCodeFile = new StreamReader(_sourceCodeFileName);
            Lexer lexer  = new Lexer(sourceCodeFile);
        }

        private static void PrintGrammarView(GrammarStream istream, TextWriter stream)
        {
            Grammar grammar = new Grammar(istream.GetGrammarText());
            var grammarView = new GrammarView(grammar);
            grammarView.View(stream);
            stream.Flush();
            LRClosureTable lRClosureTable = new LRClosureTable(grammar);
            var closureView = new ClosureTableView(lRClosureTable);
            closureView.View(stream);
            stream.Flush();

            var lrTable = new LRTable(grammar, lRClosureTable);
            var lrTableView = new LRTableView(lrTable);
            lrTableView.View(stream);
            stream.Flush();

        }
    }
}
