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
            { TermType.DecimalFixedPointNumber, AstTypeEnum.Double },
            { TermType.Echo, AstTypeEnum.Echo },
            { TermType.Echoln, AstTypeEnum.Echoln },
            { TermType.Division, AstTypeEnum.Division },
            { TermType.Minis, AstTypeEnum.Minis },
            { TermType.OpeningRoundBracket, AstTypeEnum.RoundBracket },
            { TermType.ClosingRoundBracket, AstTypeEnum.RoundBracket },
        };
        public static AstTypeEnum ConvertTermTypeToAstType(TermType type)
        {
            return dictionary[type];
        }
    }
}