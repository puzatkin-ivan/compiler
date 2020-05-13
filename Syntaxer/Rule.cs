using System;
using System.Collections.Generic;

namespace Compiler.Syntaxer
{
    public class Rule
    {
        public int Index { get; private set; }
        public string NonTerminal { get; private set; }
        public string[] Development { get; private set; }

        private string _rule;

        public Rule(string rule, int index)
        {
            _rule = rule;

            Index = index;
            string[] splitRule = rule.Split("->");

            if (splitRule.Length != 2)
            {
                throw new Exception("The rule does not meet the specificatio: " + rule);
            }

            NonTerminal = splitRule[0].Trim();
            Development = splitRule[1].Trim().Split(' ');
        }

        public override string ToString()
        {
            return _rule;
        }

        public override bool Equals(Object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            Rule rhs = (Rule)obj;
            if (NonTerminal != rhs.NonTerminal)
            {
                return false;
            }

            if (Development.Length != rhs.Development.Length)
            {
                return false;
            }

            foreach (string symbol in Development)
            {
                if (!Array.Exists(rhs.Development, symbol.Equals))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}