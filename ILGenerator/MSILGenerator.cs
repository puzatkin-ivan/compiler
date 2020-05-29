using System;
using System.Collections.Generic;
using System.IO;
using compiler.Syntaxer.SyntaxTree;

namespace compiler.ILGenerator
{
    public class MSILGenerator
    {
        private Dictionary<AstTypeEnum, string> constructions = new Dictionary<AstTypeEnum, string>()
        {
            { AstTypeEnum.Plus, "add" },
            { AstTypeEnum.Multiple, "mul" },
            { AstTypeEnum.Division, "div" },
            { AstTypeEnum.Minis, "sub" },
            { AstTypeEnum.Int, "ldc.i4 {VALUE}" },
            { AstTypeEnum.Double, "ldc.r8 {VALUE}" },
            { AstTypeEnum.Echo, "call void [mscorlib]System.Console::Write({TYPE})"},
            { AstTypeEnum.Echoln, "call void [mscorlib]System.Console::WriteLine({TYPE})"},
            { AstTypeEnum.Read, "call string class [System.Console]System.Console::ReadLine()" },
            { AstTypeEnum.String, "ldstr {VALUE}"}
        };

        public void Generate(TextWriter ilsourceFile, ASTree tree)
        {
            string result = GetHeaderConstructions();

            string body = GenerateIlFromAst(tree);

            ilsourceFile.WriteLine(result.Replace("{BODY}", body));
            ilsourceFile.Flush();
        }

        private string GenerateIlFromAst(ASTree tree)
        {
            string result = "";
            if (tree.Left != null)
            {
                result += GenerateIlFromAst(tree.Left);
            }

            if (tree.Right != null)
            {
                result += GenerateIlFromAst(tree.Right);
            }

            if (constructions.ContainsKey(tree.Type))
            {
                result += constructions[tree.Type].Replace("{VALUE}", tree.Value).Replace("{TYPE}", tree.GetTypeConvertedIlType());
            }

            result += "\n";
            return result;
        }

        private string GetHeaderConstructions()
        {
            string result = "";

            var headerFile = new StreamReader("./ILGenerator/ILConstructions/header.il");
            while (headerFile.Peek() > -1)
            {
                result += headerFile.ReadLine() + "\n";
            }

            return result;
        }
    }
}