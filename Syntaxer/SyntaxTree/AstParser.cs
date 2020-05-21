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
            AstTypeEnum.Echo,
            AstTypeEnum.Echoln,
            AstTypeEnum.RoundBracket,
        };

        public ASTree Parse(ParsingTree tree)
        {
            var astTree = GenerateASTFromParsingTree(tree);
            ConvertTypeByAstTree(astTree);
            return astTree;
        }

        private void ConvertTypeByAstTree(ASTree astTree)
        {
            ConvertTypeByAstTreeRecursive(astTree);
        }

        private AstTypeEnum ConvertTypeByAstTreeRecursive(ASTree astTree)
        {
            if (AstOperators.Contains(astTree.Type))
            {
                AstTypeEnum lhs = AstTypeEnum.Int;
                AstTypeEnum rhs = AstTypeEnum.Int;

                if (astTree.Left != null)
                {
                    lhs = ConvertTypeByAstTreeRecursive(astTree.Left);
                }

                if (astTree.Right != null)
                {
                    rhs = ConvertTypeByAstTreeRecursive(astTree.Right);
                }

                AstTypeEnum main = lhs.CompareTo(rhs) <= 0 ? rhs : lhs;

                ChangeChildType(astTree, main);

                return main;
            }
            else
            {
                return astTree.Type;
            }
        }

        private void ChangeChildType(ASTree astTree, AstTypeEnum newType)
        {
            if (astTree.Left != null)
            {
                ChangeChildType(astTree.Left, newType);
            }

            if (astTree.Right != null)
            {
                ChangeChildType(astTree.Right, newType);
            }

            if (!AstOperators.Contains(astTree.Type))
            {
                astTree.Type = newType;
            }
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

                if (nodes.Count == 3)
                {
                    operatorNode.Left = nodes[1];
                }
                else if (nodes.Count < 2)
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