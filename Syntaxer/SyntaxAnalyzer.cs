using System.IO;

using Compiler.Syntaxer;
using Compiler.Syntaxer.ClosureTable;
using Compiler.Syntaxer.Table;
using Compiler.LexerAnalyzer;
using System;
using System.Collections.Generic;
using compiler.Syntaxer.ParsingStackItem;
using compiler.Syntaxer.SyntaxTree;

namespace Compiler.Syntaxer
{
    public class SyntaxAnalyzer
    {
        private const string Epsilon = "\'\'";
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

        public ASTree Analyze(Lexer lexer, bool debug, TextWriter writer)
        {
            if (debug)
            {
                PrintStackTraceHeader(writer);
            }

            List<Node> stack = new List<Node>() { new Node(0) };
            int tokenIndex = 0;
            Node node = new Node(lexer.NextLexem(tokenIndex));
            var stateIndex = stack[ 2 * ((stack.Count - 1) >> 1)].Index;
            var state = _lrTable.States[stateIndex];
            var action = state.GetActionByNode(node);

            int index = 1;
            if (debug)
            {
                PrintStackTrace(writer, index, stack, action, lexer, tokenIndex);
            }
            while (action != null && action.ToString() != "r0")
            {
                if (action.Type == "s")
                {
                    stack.Add(node);
                    tokenIndex++;
                    stack.Add(new Node(action.Value));
                }
                else if (action.Type == "r")
                {
                    int ruleIndex = action.Value;
                    Rule rule = _grammar.Rules[ruleIndex];

                    int removeCount = Array.Exists(rule.Development, Epsilon.Equals) ? 0 : rule.Development.Length * 2;
                    var range = stack.GetRange(stack.Count - removeCount, removeCount);
                    ParsingTree tree = new ParsingTree(rule.NonTerminal, range);
                    stack.RemoveRange(stack.Count - removeCount, removeCount);
                    stack.Add(new Node(tree));
                }
                else
                {
                    stack.Add(new Node(action.Value));
                }

                stateIndex = stack[ 2 * ((stack.Count - 1) >> 1)].Index;
                state = _lrTable.States[stateIndex];
                node = stack.Count % 2 == 0 ? stack[stack.Count - 1] : new Node(lexer.NextLexem(tokenIndex));
                action = state.GetActionByNode(node);

                ++index;

                if (debug)
                {
                    PrintStackTrace(writer, index, stack, action, lexer, tokenIndex);
                }
            }

            return new AstParser().Parse(stack[1].NonTerminal);
        }

        private void PrintStackTraceHeader(TextWriter writer)
        {
            writer.WriteLine("### Stack Trace");
            writer.WriteLine("| Step | Stack | Input | Action |");
            writer.WriteLine("| ----------- | ----------- | ----------- | ----------- |");
        }

        private void PrintStackTrace(TextWriter writer, int index, List<Node> stack, LRAction action, Lexer lexer, int tokenIndex)
        {
            writer.Write("| " + index + " | ");

            foreach (Node item in stack)
            {
                writer.Write(item.ToString() + " ");
            }
            writer.Write(" | ");

            for (int i = tokenIndex; i < lexer.LexemCount; ++i)
            {
                writer.Write(lexer.NextLexem(i) + " ");
            }
            writer.Write(" | ");

            writer.WriteLine(action.ToString() + " |");
            writer.Flush();
        }
    }
}