using System;
using System.Collections.Generic;
using System.IO;
using Compiler.Syntaxer;

namespace compiler.Syntaxer.View
{
    public class GrammarView
    {
        private Grammar _grammar;

        public GrammarView(Grammar grammar)
        {
            _grammar = grammar;
        }

        public void View(TextWriter writer)
        {
            PrintGrammarRule(writer);
            PrintGrammarFirstAndFollow(writer);
        }

        private void PrintGrammarRule(TextWriter writer)
        {
            writer.WriteLine("### SLR Grammar");

            int index = 0;
            foreach (Rule rule in _grammar.Rules)
            {
                writer.WriteLine(index++ + ") " + rule.ToString());
            }
            writer.WriteLine();
        }
        private void PrintGrammarFirstAndFollow(TextWriter writer)
        {
            writer.WriteLine("### First/Follow table");
            writer.WriteLine("Nonterminal | First | Follow |");
            writer.WriteLine("| ----------- | -----------| ----------- |");

            foreach (string nonTerminal in _grammar._nonTerminals)
            {
                writer.Write("| ");
                writer.Write(nonTerminal + " | ");

                var first = _grammar._firsts[nonTerminal];
                PrintSet(writer, first);

                writer.Write(" | ");

                var follows = _grammar._follows[nonTerminal];
                PrintSet(writer, follows);

                writer.WriteLine(" | ");
            }
        }

        private void PrintSet(TextWriter writer, ISet<string> sset)
        {
            foreach (string item in sset)
            {
                writer.Write(item + ", ");
            }
        }
    }
}