using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Lexer.Enums;

namespace Compiler.Lexer
{
    public class Lexer
    {
        private TextReader _textReader;

        private Queue<Lexem> _lexems = new Queue<Lexem>();

        public Lexer( TextReader textReader )
        {
            _textReader = textReader;

            ReadAllLexemInQueue();
        }

        public Lexem NextLexem()
        {
            Lexem lexem;
            return _lexems.TryDequeue(out lexem) ? lexem : null;
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
        }

        private void ProcessSourceLine(Queue<Lexem> lexemStack, string line, int rowPosition )
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
                Console.WriteLine(charNumber);
                Console.WriteLine(line.Length);
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
            Console.WriteLine(lexem);
            Console.WriteLine(lexem.Length);

            return new Lexem(lexem, rowPosition);
        }

        private void AddTermInQueue( Queue<Lexem> queue, Lexem term )
        {
            if (term != null && term.Length() != 0)
            {
                queue.Enqueue(term);
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
