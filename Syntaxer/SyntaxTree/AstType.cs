using System.Collections.Generic;
using Compiler.LexerAnalyzer.Enums;

namespace compiler.Syntaxer.SyntaxTree
{
    public class AstType
    {
        private static Dictionary<TermType, AstTypeEnum> dictionary = new Dictionary<TermType, AstTypeEnum>() {
            { TermType.Plus, AstTypeEnum.Plus },
            { TermType.Multiple, AstTypeEnum.Multiple },
            { TermType.DecimalWholeNumber, AstTypeEnum.Int },
            { TermType.DecimalFloatingPointNumber, AstTypeEnum.Double },
            { TermType.Echo, AstTypeEnum.Echo }
        };
        public static AstTypeEnum ConvertTermTypeToAstType(TermType type)
        {
            return dictionary[type];
        }
    }
}