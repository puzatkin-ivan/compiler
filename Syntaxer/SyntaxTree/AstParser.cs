using System;
using System.Collections.Generic;
using compiler.Syntaxer.ParsingStackItem;
using Compiler.LexerAnalyzer.Enums;

namespace compiler.Syntaxer.SyntaxTree
{
    public class AstParser
    {
        private static List<AstTypeEnum> AstOperators = new List<AstTypeEnum>() {
            AstTypeEnum.Multiple,
            AstTypeEnum.Plus,
            AstTypeEnum.Division,
            AstTypeEnum.Minis,
            AstTypeEnum.Echo,
            AstTypeEnum.Echoln,
            AstTypeEnum.RoundBracket,
            AstTypeEnum.Read,
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
                if (pTree.ChildNodes.Count == 1)
                {
                    return GenerateASTFromParsingTree(pTree.ChildNodes[0]);
                }

                List<ASTree> operatorNodes = new List<ASTree>();
                List<ASTree> nodes = new List<ASTree>();

                foreach (var child in pTree.ChildNodes)
                {
                    var newNode = GenerateASTFromParsingTree(child);

                    if (AstOperators.Contains(newNode.Type) && newNode.Left == null && newNode.Right == null)
                    {
                        operatorNodes.Add(newNode);
                    }
                    else if (newNode != null)
                    {
                        nodes.Add(newNode);
                    }
                }

                ASTree operatorNode = null;

                if (operatorNodes.Count == 1)
                {
                    operatorNode = operatorNodes[0];
                    if (nodes.Count == 3)
                    {
                        operatorNode.Left = nodes[1];
                    }
                    else if (nodes.Count == 1)
                    {
                        operatorNode.Left = nodes[0];
                    }
                    else if (nodes.Count == 2)
                    {
                        operatorNode.Left = nodes[0];
                        operatorNode.Right = nodes[1];
                    }
                }
                else if (operatorNodes.Count == 2)
                {
                    operatorNode = operatorNodes[0];
                    operatorNode.Left = operatorNodes[1];
                    if (nodes.Count == 3)
                    {
                        operatorNode.Right = nodes[1];
                    }
                    else if (nodes.Count == 1)
                    {
                        operatorNode.Right = nodes[0];
                    }
                } 
                else
                {
                    operatorNode = operatorNodes[1];
                }


                return operatorNode; 
            }
        }

        private ASTree GenerateAstByLeaf(LexemParsingTree tree)
        {
            var lexem = tree.NonTerminal;

            if (lexem.Type == TermType.InstructionEnd)
            {
                return null;
            }
            return new ASTree(AstType.ConvertTermTypeToAstType(lexem.Type), lexem.Value);
        }
    }
}