using System;

namespace compiler.Syntaxer.SyntaxTree
{
    public class ASTree
    {
        public AstTypeEnum Type { get; set; }
        public string Value { get; private set; }
        public ASTree Left { get; set; }
        public ASTree Right { get; set; }

        public ASTree(AstTypeEnum type, string value)
        {
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

        public string GetTypeConvertedIlType()
        {
            string lhs = "";
            string rhs = "";
            string main = "";

            if (Left != null)
            {
                lhs = Left.GetTypeConvertedIlType();
            }

            if (Right != null)
            {
                rhs = Right.GetTypeConvertedIlType();
            }

            switch (Type)
            {
                case AstTypeEnum.Int:
                    main = "int32";
                    break;
                case AstTypeEnum.Double:
                    main = "float64";
                    break;
                case AstTypeEnum.String:
                case AstTypeEnum.Read:
                    main = "string";
                    break;
                default:
                    break;
            }

            if (lhs.Length < rhs.Length)
            {
                return rhs.Length < main.Length ? main : rhs;
            }
            else
            {
                return lhs.Length < main.Length ? main : lhs;
            }
        }
    }
}