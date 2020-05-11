using System.IO;

namespace Compiler.Syntaxer
{
    public class GrammarStream
    {
        private StreamReader _stream;

        private string _grammarText;

        public GrammarStream(StreamReader grammarStream)
        {
            _stream = grammarStream;

            _grammarText = "";
        }

        public string GetGrammarText()
        {
            if (_grammarText.Length == 0)
            {
                while (_stream.Peek() >= 0)
                {
                    _grammarText += _stream.ReadLine() + "\n";
                }
            }

            return _grammarText;
        }
    }
}