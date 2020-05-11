using System;
using System.Collections.Generic;
using System.Text;
using Compiler.Lexer.Enums;

namespace Compiler.Lexer
{
    public class Lexem
    {
        public string Value { get; private set; }
        public TermType Type { get; private set; }
        public int RowPosition { get; private set; }

        public Lexem( string lexem, int rowPosition)
        {
            Value = lexem;
            RowPosition = rowPosition;
            Type = TermRecognizer.GetTypeByTermString( lexem );
        }

        public Lexem(TermType type, int rowPosition)
        {
            Value = "";
            RowPosition = rowPosition;
            Type = type;
        }

        public int Length()
        {
            return Value.Length;
        }
    }
}
