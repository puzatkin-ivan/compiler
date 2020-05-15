using Compiler.LexerAnalyzer;

namespace compiler.Syntaxer.ParsingStackItem
{
    public class Node
    {
        public int Index { get; set; }
        public Lexem Lexem { get; set; }
        public ParsingTree NonTerminal { get; set; }

        public Node(int index)
        {
            Index = index;
            Lexem = null;
            NonTerminal = null;
        }

        public Node(Lexem lexem)
        {
            Index = -1;
            Lexem = lexem;
            NonTerminal = null;
        }

        public Node(ParsingTree nonTerminal)
        {
            Index = -1;
            Lexem = null;
            NonTerminal = nonTerminal;
        }

        public Node(Node node)
        {
            Index = node.Index;
            Lexem = node.Lexem;
            NonTerminal = node.NonTerminal;
        }

        public override string ToString()
        {
            if (Index != -1)
            {
                return Index.ToString();
            }

            if (Lexem != null)
            {
                return Lexem.Value;
            }
            else
            {
                return NonTerminal.NonTerminal;
            }
        }
    }
}