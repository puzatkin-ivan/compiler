using System;
using System.Collections.Generic;
using Compiler.LexerAnalyzer;

namespace compiler.Syntaxer.ParsingStackItem
{
    public interface Tree
    {
        string ToString();
        string ToString(string prefix);
    } 

    public class ParsingTree : Tree
    {
        public string NonTerminal { get; private set; }
        public List<Tree> ChildNodes { get; private set; }

        public ParsingTree(string nonTerminal, List<Node> childNodes)
        {
            NonTerminal = nonTerminal;
            ChildNodes = new List<Tree>();

            for (int index = 0; index < childNodes.Count; index += 2)
            {
                if (childNodes[index].Lexem != null)
                {
                    ChildNodes.Add(new LexemParsingTree(childNodes[index].Lexem));
                }

                if (childNodes[index].NonTerminal != null)
                {
                    ChildNodes.Add(childNodes[index].NonTerminal);
                } 
            }
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string prefix)
        {
            string result = prefix + "- " + NonTerminal + "\n";

            prefix += "  ";
            if (ChildNodes.Count > 0)
            {
                foreach (var tree in ChildNodes)
                {
                    result += prefix + tree.ToString(prefix);
                }
            }
            return result;
        }
    }

    public class LexemParsingTree : Tree
    {
        public Lexem NonTerminal;

        public LexemParsingTree(Lexem lexem)
        {
            NonTerminal = lexem;
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string prefix)
        {
            return prefix + "- '" + NonTerminal.ToString() + "'\n";
        }
    }
}