using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Compiler.Syntaxer.ClosureTable
{
    public class LRKernel
    {
        public int Index { get; private set; }
        public List<LRKernelItem> Items { get; private set; }
        public List<LRKernelItem> Closure { get; private set; }
        public List<string> Keys { get; set; }
        public Dictionary<string, int> Gotos { get; set; }

        public LRKernel(int index, List<LRKernelItem> items)
        {
            Index = index;
            Items = items;
            Closure = new List<LRKernelItem>(items); // maybe in these parts must be magic js: items.slice(0);
            Gotos = new Dictionary<string, int>();
            Keys = new List<string>();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var rhs = (LRKernel) obj;
            return IncludeEachOtherUsingEquals(this.Items, rhs.Items);
        }

        private bool IncludeEachOtherUsingEquals(List<LRKernelItem> lhs, List<LRKernelItem> rhs)
        {
            return IncludesUsingEquals(lhs, rhs) && IncludesUsingEquals(rhs, lhs);
        }

        private bool IncludesUsingEquals(List<LRKernelItem> lhs, List<LRKernelItem> rhs)
        {
            foreach (LRKernelItem item in lhs)
            {
                if (IndexOfUsingEquals(item, rhs) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private int IndexOfUsingEquals(LRKernelItem first, List<LRKernelItem> rhs)
        {
            return rhs.FindIndex(first.Equals);
        }

        public override string ToString()
        {
            string result = "";
            foreach (var item in Items)
            {
                result += item.ToString() + " ";
            }

            string resultClosure = "";
            foreach (var closure in Closure)
            {
                resultClosure += closure.ToString() + " ";
            }
            return "Closure { " + result + "} = { " + resultClosure + "}" ;
        }
    }
}