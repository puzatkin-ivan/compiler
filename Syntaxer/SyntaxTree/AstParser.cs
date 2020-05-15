using System;
using System.Collections.Generic;
using compiler.Syntaxer.ParsingStackItem;

namespace compiler.Syntaxer.SyntaxTree
{
    public class AstParser
    {
        private static List<AstTypeEnum> AstOperators = new List<AstTypeEnum>() {
            AstTypeEnum.Multiple,
            AstTypeEnum.Plus,
        };

        public ASTree Parse(ParsingTree tree)
        {
            return GenerateASTFromParsingTree(tree);
        }

        private ASTree GenerateASTFromParsingTree(Tree tree)
        {
            if (tree.GetType() == typeof(LexemParsingTree))
            {
                return GenerateAstByLeaf((LexemParsingTree)tree);
            }
            else
            {
                ParsingTree pTree = (ParsingTree)tree;
                if (pTree.ChildNodes.Count < 2)
                {
                    return GenerateASTFromParsingTree(pTree.ChildNodes[0]);
                }

                ASTree operatorNode = null;
                List<ASTree> nodes = new List<ASTree>();

                foreach (var child in pTree.ChildNodes)
                {
                    var newNode = GenerateASTFromParsingTree(child);

                    if (AstOperators.Contains(newNode.Type) && newNode.Left == null && newNode.Right == null)
                    {
                        operatorNode = newNode;
                    }
                    else
                    {
                        nodes.Add(newNode);
                    }
                }

                if (nodes.Count < 2)
                {
                    operatorNode.Left = nodes[0];
                }
                else
                {
                    operatorNode.Left = nodes[0];
                    operatorNode.Right = nodes[1];
                }

                return operatorNode; 
            }
        }

        private ASTree GenerateAstByLeaf(LexemParsingTree tree)
        {
            var lexem = tree.NonTerminal;
            return new ASTree(AstType.ConvertTermTypeToAstType(lexem.Type), lexem.Value);
        }
    }
}