namespace Compiler.Syntaxer.Table
{
    public class LRAction
    {
        public string Type { get; private set; }

        public int Value { get; private set; }

        public LRAction(string type, int value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return Type + Value;
        }
    }
}