﻿using System;
using System.Collections.Generic;
using System.Linq;
using Compiler.Lexer.Enums;

namespace Compiler.Lexer
{
    public class TermRecognizer
    {
        private delegate bool IsDigit( char letter );

        public static TermType GetTypeByTermString( string termString )
        {
            if ( termString == null || !termString.Any() )
                throw new ArgumentException( "Term can't be empty" );
            if ( ReservedWordTypeByString.ContainsKey( termString ) )
                return ReservedWordTypeByString[ termString ];

            if ( termString.Length == 1 && DelimeterTypeByString.ContainsKey( termString ) )
            {
                return DelimeterTypeByString[ termString ];
            }

            if ( IsString(termString.First(), termString.Last()) )
            {
                return TermType.String;
            }

            if ( IsEnglishLetter( termString.First() ) )
            {
                return GetIdentifierType( termString );
            }

            if ( char.IsDigit( termString[ 0 ] ) )
            {
                if ( termString.Length > 255 )
                    return TermType.Error;
                return GetNumberType( termString );
            }

            return TermType.Error;
        }

        public static readonly Dictionary<string, TermType> ReservedWordTypeByString = new Dictionary<string, TermType>
        {
             { "Read", TermType.Read },
             { "Write", TermType.Write },
             { "public", TermType.Modificator },
             { "private", TermType.Modificator },
             { "protected", TermType.Modificator },
             { "if", TermType.If },
             { "else", TermType.Else },
             { "while", TermType.While },
             { "do", TermType.Do },
             { "for", TermType.For },
             { "char", TermType.Char },
             { "bool", TermType.Bool },
             { "double", TermType.Double },
             { "void", TermType.Void },
             { "var", TermType.Var },
             { "int", TermType.Int },
             { "float", TermType.Float },
             { "[END]", TermType.End },
             { "true", TermType.Bool },
             { "false", TermType.Bool },
             { "List", TermType.List },
             { "string", TermType.StringType },
             { "main", TermType.Main },
             { "@@@", TermType.DEnd },
             { "Array", TermType.Array },
             { "IntArray", TermType.IntArray },
             { "div", TermType.Division },
             { "Echo", TermType.Echo },
             { "Echoln", TermType.Echoln },
            { "&&", TermType.And },
            { "||", TermType.Or },
            { "==", TermType.Equal },
            { "!=", TermType.NotEqual },
            { ">=", TermType.MoreEqual },
            { "<=", TermType.LessEqual },
            { "|", TermType.BExp }
        };

        public static readonly Dictionary<string, TermType> DelimeterTypeByString = new Dictionary<string, TermType>
        {
             { "{", TermType.OpeningBrace },
             { "}", TermType.ClosingBrace },
             { "(", TermType.OpeningRoundBracket },
             { ")", TermType.ClosingRoundBracket },
             { ";", TermType.InstructionEnd },
             { "+", TermType.Plus },
             { "-", TermType.Minis },
             { ":", TermType.Colon },
             { ",", TermType.Comma },
             { "*", TermType.Multiple },
             { "$", TermType.Dollar },
             { "=", TermType.Equally },
             { ">", TermType.More },
             { "<", TermType.Less },
             { ".", TermType.Point },
             { "[", TermType.OpeningSquareBrace },
             { "]", TermType.ClosingSquareBrace },
             { "!", TermType.ExclamationMark },
            { "`", TermType.Tilda }
        };

        public static readonly Dictionary<KeyValuePair<CommentState, char>, CommentState> CommentStateMachine = new Dictionary<KeyValuePair<CommentState, char>, CommentState>
        {
             { KeyValuePair.Create(CommentState.NormalText, '/'), CommentState.CommitBegining },
             { KeyValuePair.Create(CommentState.CommitBegining, '/'), CommentState.OneLineCommit },
             { KeyValuePair.Create(CommentState.CommitBegining, '*'), CommentState.MultiLineCommit },
             { KeyValuePair.Create(CommentState.MultiLineCommit, '*'), CommentState.CommitEnding },
             { KeyValuePair.Create(CommentState.CommitEnding, '/'), CommentState.CommitEnd }
        };

        private static readonly Dictionary<KeyValuePair<NumberState, DigitType>, NumberState> NumberStateMachine = new Dictionary<KeyValuePair<NumberState, DigitType>, NumberState>
        {
             { KeyValuePair.Create(NumberState.FirstSign, DigitType.Digit), NumberState.Digit },
             { KeyValuePair.Create(NumberState.Digit, DigitType.Digit), NumberState.Digit },
             { KeyValuePair.Create(NumberState.Digit, DigitType.E), NumberState.E },
             { KeyValuePair.Create(NumberState.Digit, DigitType.Point), NumberState.Point },
             { KeyValuePair.Create(NumberState.Point, DigitType.Digit), NumberState.DigitAfterPoint },
             { KeyValuePair.Create(NumberState.E, DigitType.Sign), NumberState.Sign },
             { KeyValuePair.Create(NumberState.Sign, DigitType.Digit), NumberState.DigitAfterSign },
             { KeyValuePair.Create(NumberState.E, DigitType.Digit), NumberState.DigitAfterSign },
             { KeyValuePair.Create(NumberState.DigitAfterPoint, DigitType.E), NumberState.E },
             { KeyValuePair.Create(NumberState.DigitAfterPoint, DigitType.Digit), NumberState.DigitAfterPoint },
             { KeyValuePair.Create(NumberState.DigitAfterSign, DigitType.Digit), NumberState.DigitAfterSign }

        };

        public static readonly Dictionary<KeyValuePair<NumberSystemType, NumberRankType>, TermType> TermTypeByNumberType = new Dictionary<KeyValuePair<NumberSystemType, NumberRankType>, TermType>
        {
             { KeyValuePair.Create(NumberSystemType.Binary, NumberRankType.Whole), TermType.BinaryWholeNumber },
             { KeyValuePair.Create(NumberSystemType.Binary, NumberRankType.FloatPoint), TermType.BinaryFloatingPointNumber },
             { KeyValuePair.Create(NumberSystemType.Binary, NumberRankType.FixedPoint), TermType.BinaryFixedPointNumber },
             { KeyValuePair.Create(NumberSystemType.Octal, NumberRankType.Whole), TermType.OctalWholeNumber },
             { KeyValuePair.Create(NumberSystemType.Octal, NumberRankType.FloatPoint), TermType.OctalFloatingPointNumber },
             { KeyValuePair.Create(NumberSystemType.Octal, NumberRankType.FixedPoint), TermType.OctalFixedPointNumber },
             { KeyValuePair.Create(NumberSystemType.Hexadecimal, NumberRankType.Whole), TermType.HexadecimalWholeNumber },
             { KeyValuePair.Create(NumberSystemType.Hexadecimal, NumberRankType.FloatPoint), TermType.HexadecimalFloatingPointNumber },
             { KeyValuePair.Create(NumberSystemType.Hexadecimal, NumberRankType.FixedPoint), TermType.HexadecimalFixedPointNumber },
             { KeyValuePair.Create(NumberSystemType.Decimal, NumberRankType.Whole), TermType.DecimalWholeNumber },
             { KeyValuePair.Create(NumberSystemType.Decimal, NumberRankType.FloatPoint), TermType.DecimalFloatingPointNumber },
             { KeyValuePair.Create(NumberSystemType.Decimal, NumberRankType.FixedPoint), TermType.DecimalFixedPointNumber },
        };

        public static bool IsNumber( TermType termType )
        {
            return termType >= TermType.BinaryWholeNumber && termType <= TermType.DecimalFloatingPointNumber;
        }

        public static bool IsIdentifier( string word )
        {
            if ( word.Length == 0 )
                return false;
            if ( char.IsDigit( word[ 0 ] ) )
                return false;

            for ( int i = 1; i < word.Length; i++ )
            {
                char letter = word[ i ];
                if ( !IsEnglishLetter( letter ) && !char.IsDigit( letter ) )
                    return false;
            }

            return true;
        }

        public static HashSet<char> Delimeters = new HashSet<char> { '(', ')', '{', '}', ';', ' ', '\t', '+', '-', ':', ',', '*', '$', '[', ']', '`' };

        public static char StartOrEndString = '"';

        private static TermType GetNumberType( string termString )
        {
            NumberSystemType systemType = GetNumberSystemType( termString );
            if ( systemType == NumberSystemType.Error )
                return TermType.Error;

            NumberRankType rankType = GetNumberRankType( termString, IsDigitByNumberSystemType( systemType ) );
            if ( rankType == NumberRankType.Error )
                return TermType.Error;

            var numberTypeKey = KeyValuePair.Create( systemType, rankType );

            return TermTypeByNumberType[ numberTypeKey ];
        }

        private static TermType GetIdentifierType( string termString )
        {
            bool isIdentifier = IsIdentifier( termString );
            if ( isIdentifier )
                return ReservedWordTypeByString.ContainsKey( termString )
                    ? ReservedWordTypeByString[ termString ]
                    : TermType.Identifier;

            return TermType.Error;
        }

        private static bool IsEnglishLetter( char letter )
        {
            int intCode = Convert.ToInt32( letter );
            if ( intCode >= 63 && intCode <= 126 )
                return true;

            return false;
        }

        private static DigitType LetterToDigitType( char letter, IsDigit IsDigit )
        {
            if ( letter == 'E' )
                return DigitType.E;
            else if ( IsDigit != null && IsDigit( letter ) )
                return DigitType.Digit;
            else if ( letter == '.' )
                return DigitType.Point;
            else if ( letter == '+' )
                return DigitType.Sign;
            else if ( letter == '-' )
                return DigitType.Sign;

            return DigitType.Error;
        }

        private static int GetDigitBegining( string termString )
        {
            List<string> beginings = new List<string> { "0B", "0O", "0H" };
            if ( beginings.Contains( new string( termString.Take( 2 ).ToArray() ) ) )
                return 2;

            return 0;
        }

        private static NumberSystemType GetNumberSystemType( string termString )
        {
            Dictionary<char, NumberSystemType> systemTypeByLetter = new Dictionary<char, NumberSystemType>
            {
                { 'B',  NumberSystemType.Binary},
                { 'O',  NumberSystemType.Octal},
                { 'H',  NumberSystemType.Hexadecimal},
            };

            if ( termString.Length == 1 )
                return NumberSystemType.Decimal;

            char letter = termString[ 1 ];
            if ( systemTypeByLetter.ContainsKey( letter ) )
            {
                if ( termString.Length <= 2 )
                    return NumberSystemType.Error;
                return systemTypeByLetter[ letter ];
            }

            int i = GetDigitBegining( termString );
            letter = termString[ i ];
            if ( char.IsDigit( letter ) )
                return NumberSystemType.Decimal;

            return NumberSystemType.Error;
        }

        private static NumberRankType GetNumberRankType( string termString, IsDigit IsDigit )
        {
            const int eMaxSize = 255;

            var state = NumberState.Digit;

            var eValue = "";
            for ( int i = GetDigitBegining( termString ); i < termString.Length; i++ )
            {
                char letter = termString[ i ];
                DigitType digitType = LetterToDigitType( letter, IsDigit );
                if ( digitType == DigitType.Error )
                    return NumberRankType.Error;
                var numberStateMachineKey = KeyValuePair.Create( state, digitType );

                if ( !NumberStateMachine.ContainsKey( numberStateMachineKey ) )
                    return NumberRankType.Error;

                state = NumberStateMachine[ numberStateMachineKey ];
                if ( state == NumberState.DigitAfterSign && letter != 'E' )
                {
                    eValue += letter;
                    if ( int.Parse( eValue ) > eMaxSize )
                        return NumberRankType.Error;
                }
            }

            if ( state == NumberState.Digit )
                return NumberRankType.Whole;
            if ( state == NumberState.DigitAfterPoint )
                return NumberRankType.FixedPoint;
            if ( state == NumberState.DigitAfterSign )
                return NumberRankType.FloatPoint;

            return NumberRankType.Error;
        }

        private static bool IsHexDigit( char letter )
        {
            bool result = ( letter >= '0' && letter <= '9' ) ||
                       ( letter >= 'a' && letter <= 'f' ) ||
                       ( letter >= 'A' && letter <= 'F' );

            return result;
        }

        private static bool IsBinary( char letter )
        {
            return letter == '0' || letter == '1';
        }

        private static bool IsOctalDigit( char letter )
        {
            return letter >= '0' && letter <= '7';
        }

        private static IsDigit IsDigitByNumberSystemType( NumberSystemType numberSystemType )
        {
            if ( numberSystemType == NumberSystemType.Decimal )
                return char.IsDigit;

            if ( numberSystemType == NumberSystemType.Binary )
                return IsBinary;

            if ( numberSystemType == NumberSystemType.Octal )
                return IsOctalDigit;

            if ( numberSystemType == NumberSystemType.Hexadecimal )
                return IsHexDigit;

            return null;
        }

        private static bool IsString(char start, char end)
        {
            return start == TermRecognizer.StartOrEndString && end == TermRecognizer.StartOrEndString;
        }
    }
}
