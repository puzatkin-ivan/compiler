using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.LexerAnalyzer.Enums;

namespace Compiler.LexerAnalyzer
{
    public class Lexer
    {
        public int LexemCount { get; private set; }
        private TextReader _textReader;

        private List<Lexem> _lexems = new List<Lexem>();

        public Lexer( TextReader textReader )
        {
            _textReader = textReader;

            ReadAllLexemInQueue();
            LexemCount = _lexems.Count;
        }

        public Lexem NextLexem(int tokenIndex)
        {
            return _lexems[tokenIndex];
        }

        private void ReadAllLexemInQueue()
        {
            int rowPosition = 1;
            string line = _textReader.ReadLine();

            while (line != null)
            {
                ProcessSourceLine(_lexems, line, rowPosition);
                rowPosition++;
                line = _textReader.ReadLine();
            }

            AddTermInQueue(_lexems, new Lexem("$", rowPosition));
        }

        private void ProcessSourceLine(List<Lexem> lexemStack, string line, int rowPosition )
        {
            var wordBuilder = new StringBuilder();

            string word;
            for ( int charNumber = 0; charNumber < line.Length; charNumber++ )
            {
                char letter = line[ charNumber ];
                if ( letter == TermRecognizer.StartOrEndString)
                {
                    int offset;
                    AddTermInQueue(lexemStack, CreateStringTermFromString(line, rowPosition, charNumber, out offset));
                    charNumber += offset;
                }
                else if ( TermRecognizer.Delimeters.Contains( letter ))
                {
                    word = wordBuilder.ToString();

                    AddTermInQueue(lexemStack, CreateTermFromString(word, rowPosition));

                    AddTermInQueue(lexemStack, CreateDelimeterTermFromChar(letter, rowPosition));
                    
                    wordBuilder.Clear();
                }
                else
                {
                    wordBuilder.Append( letter );
                }
            }

            word = wordBuilder.ToString();

            AddTermInQueue(lexemStack, CreateTermFromString(word, rowPosition));
        }

        private Lexem CreateStringTermFromString(string line, int rowPosition, int startIndex, out int offset)
        {
            if (line.Length <= startIndex)
            {
                offset = 0;
                return null;
            }

            StringBuilder lexemBuilder = new StringBuilder();

            lexemBuilder.Append(line[startIndex]);
            int index = startIndex + 1;
            while (line[index] != '"' && index < line.Length)
            {
                lexemBuilder.Append(line[index]);
                index++;
            }
            lexemBuilder.Append(line[index]);

            string lexem = lexemBuilder.ToString();

            offset = lexem.Length - 1;

            return new Lexem(lexem, rowPosition);
        }

        private void AddTermInQueue( List<Lexem> queue, Lexem term )
        {
            if (term != null && term.Length() != 0)
            {
                queue.Add(term);
            }
        }

        private Lexem CreateTermFromString(string word, int rowPosition)
        {
            int wordLength = word.Length;

            if (wordLength == 0)
            {
                return null;
            }

            return new Lexem(word, rowPosition);
        }

        private Lexem CreateDelimeterTermFromChar( char delimeter, int rowPosition  )
        {
            if ( delimeter != ' ' && delimeter != '\t' )
            {
                return new Lexem(delimeter.ToString(), rowPosition);
            }

            return null;
        }
    }
}
