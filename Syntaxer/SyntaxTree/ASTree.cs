using System;

namespace compiler.Syntaxer.SyntaxTree
{
    public class ASTree
    {
        public AstTypeEnum Type { get; private set; }
        public string Value { get; private set; }
        public ASTree Left { get; set; }
        public ASTree Right { get; set; }

        public ASTree(AstTypeEnum type, string value)
        {
            Console.WriteLine(type + value);
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string prefix)
        {
            string result = prefix + "- '" + Value + "'\n";

            prefix += "  ";
            if (Left != null)
            {
                result += prefix + Left.ToString(prefix);
            }

            if (Right != null)
            {
                result += prefix + Right.ToString(prefix);
            }
            return result;
        }
    }
}