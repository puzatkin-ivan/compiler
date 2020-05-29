using System;
using System.IO;

namespace Replacer
{
    class Program
    {
        static void Main(string[] args)
        {
            TextReader reader = new StreamReader("../../out/lsdlang.il");

            string result = "";
            while (reader.Peek() > -1)
            {
                result += reader.ReadLine().Replace("double", "float64") + "\n";
            }

            StreamWriter writer = new StreamWriter("../../out/lsdlang.il");

            writer.WriteLine(result);
            writer.Flush();
            writer.Close();
        }
    }
}
