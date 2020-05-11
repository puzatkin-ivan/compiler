using System;
using System.Collections.Generic;

namespace Compiler.Syntaxer
{
    public class Grammar
    {
        private const string Epsilon = "\'\'";
        
        public List<Rule> Rules { get; private set; }

        private readonly string _lang;
        private string _axiom;
        private readonly ISet<string> _alphabet;
        private readonly ISet<string> _nonTerminals;
        public ISet<string> _terminals { get; private set; }

        public Dictionary<string, ISet<string>> _firsts;
        public Dictionary<string, ISet<string>> _follows;

        public Grammar(string lang)
        {
            _lang = lang;
            Rules = new List<Rule>();
            _alphabet = new HashSet<string>();
            _terminals = new HashSet<string>();
            _nonTerminals = new HashSet<string>();
            _firsts = new Dictionary<string, ISet<string>>();
            _follows = new Dictionary<string, ISet<string>>();

            InitializeRulesAndAlphabetAndNonterminals();
            InitializeAlphabetAndTerminals();
            InitializeFirsts();
            InitializeFollows();
        }
        
        public List<Rule> GetRulesForNonterminalByDotIndex(Rule rule, int dotIndex)
        {
            if (dotIndex >= rule.Development.Length)
            {
                return new List<Rule>();
            }

            var nonterminal = rule.Development[dotIndex];

            var result = new List<Rule>();
            foreach (Rule l_rule in Rules)
            {
                if (nonterminal == l_rule.NonTerminal)
                {
                    result.Add(rule);
                }
            }

            return result;
        }

        private void InitializeRulesAndAlphabetAndNonterminals()
        {
            string[] lines = _lang.Split('\n');

            for (int index = 0; index < lines.Length; ++index)
            {
                string line = lines[index].Trim();

                if (line.Length != 0)
                {
                    Rule rule = new Rule(line, Rules.Count);

                    Rules.Add(rule);

                    string nonTerminal = rule.NonTerminal;
                    if (_axiom == null)
                    {
                        _axiom = nonTerminal;
                    }

                    _alphabet.Add(nonTerminal);
                    _nonTerminals.Add(nonTerminal);
                }
            }
        }

        private void InitializeAlphabetAndTerminals()
        {
            foreach (Rule rule in Rules)
            {
                foreach (string symbol in rule.Development)
                {
                    if (symbol != Epsilon && !_nonTerminals.Contains(symbol))
                    {
                        _alphabet.Add(symbol);
                        _terminals.Add(symbol);
                    }
                }
            }

        }

        private void InitializeFirsts()
        {
            bool notDone;

            do {
                notDone = false;

                foreach (Rule rule in Rules)
                {
                    string nonTerminal = rule.NonTerminal;

                    if (!_firsts.ContainsKey(nonTerminal))
                    {
                        _firsts.Add(nonTerminal, new HashSet<string>());
                    }

                    if (rule.Development.Length == 1 && rule.Development[0] == Epsilon)
                    {
                        notDone |= _firsts[nonTerminal].Add(Epsilon);
                    }
                    else
                    {
                        notDone |= CollectDevelopmentFirsts(rule);
                    }
                }

            } while (notDone);
        }

        private bool CollectDevelopmentFirsts(Rule rule)
        {
            bool result = false;
            bool epsilonInSymbolFirsts = true;

            string nonTerminal = rule.NonTerminal;

            foreach (string symbol in rule.Development)
            {
                epsilonInSymbolFirsts = false;

                if (_terminals.Contains(symbol))
                {
                    result |= _firsts[nonTerminal].Add(symbol);
                    break;
                }

                if (_firsts.ContainsKey(symbol))
                {
                    foreach (string firstItem in _firsts[symbol])
                    {
                        epsilonInSymbolFirsts |= firstItem == Epsilon;

                        result |= _firsts[nonTerminal].Add(firstItem);
                    }
                }
            }

            if (epsilonInSymbolFirsts)
            {
                result |= _firsts[nonTerminal].Add(Epsilon);
            }

            return result;
        }

        private void InitializeFollows()
        {
            bool notDone;

            do
            {
                notDone = false;

                if (!_follows.ContainsKey(Rules[0].NonTerminal))
                {
                    _follows.Add(Rules[0].NonTerminal, new HashSet<string>());
                    notDone |= _follows[Rules[0].NonTerminal].Add("$");
                }

                foreach (Rule rule in Rules)
                {
                    for (int j = 0; j < rule.Development.Length; ++j)
                    {
                        string symbol = rule.Development[j];
                        if (_nonTerminals.Contains(symbol))
                        {
                            if (!_follows.ContainsKey(symbol))
                            {
                                _follows.Add(symbol, new HashSet<string>());
                            }

                            int len = rule.Development.Length;
                            int nextIndex = j +1;
                            string[] slicingDevelopment = new string[len - nextIndex];
                            int jindex = 0;
                            for (int jj = nextIndex; jj < len; ++jj)
                            {
                                slicingDevelopment[jindex] = rule.Development[jj];
                                ++jindex;
                            }
                            List<string> afterSymbolFirsts = GetSequenceFirsts(slicingDevelopment);

                            foreach (string first in afterSymbolFirsts)
                            {
                                if (first == Epsilon)
                                {
                                    foreach (string nonTerminalSymbol in _follows[rule.NonTerminal])
                                    {
                                        notDone |= _follows[symbol].Add(nonTerminalSymbol);
                                    }
                                }
                                else
                                {
                                    notDone |= _follows[symbol].Add(first);
                                }
                            }
                        }
                    }
                }
            } while (notDone);
        }

        private List<string> GetSequenceFirsts(string[] sequence)
        {
            List<string> result = new List<string>();
            bool epsilonInSymbolFirsts = true;

            foreach (string symbol in sequence)
            {
                epsilonInSymbolFirsts = false;

                if (_terminals.Contains(symbol))
                {
                    result.Add(symbol);
                    break;
                }

                foreach (string first in _firsts[symbol])
                {
                    epsilonInSymbolFirsts |= first == Epsilon;

                    result.Add(first);
                }

                epsilonInSymbolFirsts |= _firsts.ContainsKey(symbol) || _firsts[symbol].Count == 0;

                if (!epsilonInSymbolFirsts)
                {
                    break;
                }
            }

            if (epsilonInSymbolFirsts)
            {
                result.Add(Epsilon);
            }

            return result;
        }
    }
}