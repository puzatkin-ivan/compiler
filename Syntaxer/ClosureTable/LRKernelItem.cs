using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace compiler.Syntaxer
{
    public class LRKernelItem
    {
        private const string Epsilon = "\'\'";

        public Rule Rule { get; private set; }
        public int DotIndex { get; private set; }
        private Grammar _grammar;
        
        public LRKernelItem(Grammar grammar, Rule rule, int dotIndex)
        {
            _grammar = grammar;
            Rule = rule;
            DotIndex = dotIndex;
        }

        public List<LRKernelItem> NewItemsFromSymbolAfterDot()
        {
            List<LRKernelItem> result = new List<LRKernelItem>();

            var nonterminalRules = _grammar.GetRulesForNonterminalByDotIndex(Rule, DotIndex);

            foreach (var rule in nonterminalRules)
            {
                LRKernelItem newItem = new LRKernelItem(_grammar, rule, 0);
                int index = result.FindIndex(item => item.Equals(newItem));
                if (index < 0)
                {
                    result.Add(newItem);
                }
            }

            return result;
        }

        public LRKernelItem NewItemAfterShift()
        {
            if (DotIndex < Rule.Development.Length && Rule.Development[DotIndex] != Epsilon)
            {
                return new LRKernelItem(_grammar, Rule, DotIndex + 1); 
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var rhs = (LRKernelItem) obj;
            return Rule.Equals(rhs.Rule) && DotIndex == rhs.DotIndex;
        }

        public override string ToString()
        {
            string developmentConvertedToString = "";
            for (int index = 0; index < DotIndex; ++index)
            {
                developmentConvertedToString += Rule.Development[index] + " ";
            }
            
            return Rule.NonTerminal + " -> " + developmentConvertedToString + "." ;
        }
    }
}