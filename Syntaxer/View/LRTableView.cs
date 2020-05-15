using System;
using System.IO;
using Compiler.Syntaxer.Table;

namespace compiler.Syntaxer.View
{
    public class LRTableView
    {
        private  LRTable _table;

        public LRTableView(LRTable table)
        {
            _table = table;
        }

        public void View(TextWriter writer)
        {
            PrintHeaderTable(writer);

            for (int index = 0; index < _table.States.Count; ++index)
            {
                var state = _table.States[index];

                writer.Write("| " + index + " | ");

                foreach (string terminal in _table.Grammar._terminals)
                {
                    writer.Write(FormatAction(state, terminal, true) + " | ");
                }
                writer.Write(FormatAction(state, "$", true) + " | ");

                foreach (string nonTerminal in _table.Grammar._nonTerminals)
                {
                    writer.Write(FormatAction(state, nonTerminal, true) + " | ");
                }
                writer.WriteLine();
            }
        }

        private string FormatAction(LRState state, string terminal, bool isInTable)
        {
            if (!state.Actions.ContainsKey(terminal))
            {
                return "&nbsp;";
            }

            var action = state.Actions[terminal];
            if (action.Value == 0 & action.Type == "r")
            {
                return "acc";
            }
            return action.ToString();
        }

        private void PrintHeaderTable(TextWriter writer)
        {
            writer.WriteLine("### LR Table");
            writer.Write("| &nbsp;");
            for (int index = 0; index < _table.Grammar._terminals.Count + 1; ++index)
            {
                writer.Write(" | Action ");
            }

            for (int index = 0; index < _table.Grammar._nonTerminals.Count; ++index)
            {
                writer.Write("| Goto ");
            }
            writer.WriteLine("|");

            var counter =  _table.Grammar._terminals.Count + _table.Grammar._nonTerminals.Count + 2;
            for (int index = 0; index < counter; ++index)
            {
                writer.Write("| ----------- ");
            }
            writer.WriteLine(" |");

            writer.Write("| State | ");
            foreach ( string terminal in _table.Grammar._terminals)
            {
                writer.Write(terminal + " | ");
            }
            writer.Write("$ | ");

            foreach ( string nonTerminal in _table.Grammar._nonTerminals)
            {
                writer.Write(nonTerminal + " | ");
            }
            writer.WriteLine();
        }
    }
}