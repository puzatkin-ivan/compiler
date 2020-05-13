using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Syntaxer.ClosureTable
{
    public class LRClosureTable
    {
        private Grammar _grammar;
        public HashSet<LRKernel> Kernels { get; private set; }

        public LRClosureTable(Grammar grammar)
        {
            _grammar = grammar;
            Kernels = new HashSet<LRKernel>();

            var firstKernelItem = new LRKernelItem(_grammar, grammar.Rules.First(), 0);
            Kernels.Add(new LRKernel(0, new List<LRKernelItem>(){ firstKernelItem }));

            int index = 0;
            while (index < Kernels.Count)
            {
                var kernelList = Kernels.ToList();
                var kernel = kernelList[index];

                UpdateClosure(kernel);

                index = AddGotos(kernel) ? 0 : index + 1;
            }
        }

        private void UpdateClosure(LRKernel kernel)
        {
            if (kernel.Closure.Count == 0)
            {
                throw new System.Exception("Rule has not right part.");
            }

            int gindex = 0;
            while (gindex < kernel.Closure.Count)
            {
                var closure = kernel.Closure[gindex];

                List<LRKernelItem> newItemsFromSymbolAfterDot = closure.NewItemsFromSymbolAfterDot();

                foreach (LRKernelItem item in newItemsFromSymbolAfterDot)
                {
                    var index = kernel.Closure.FindIndex(item.Equals);
                    if (index < 0)
                    {
                        kernel.Closure.Add(item);
                    }
                }
                ++gindex;
            };
        }

        private bool AddGotos(LRKernel kernel)
        {
            bool lookAheadsPropagated = false;
            Dictionary<string, HashSet<LRKernelItem>> newKernels = new Dictionary<string, HashSet<LRKernelItem>>();

            foreach (LRKernelItem item in kernel.Closure)
            {
                LRKernelItem newItem = item.NewItemAfterShift();

                if (newItem != null)
                {
                    string symbolAfterDot = item.Rule.Development[item.DotIndex];

                    int keyIndex = kernel.Keys.FindIndex(symbolAfterDot.Equals);
                    if (keyIndex < 0)
                    {
                        kernel.Keys.Add(symbolAfterDot);   
                    }

                    if (!newKernels.ContainsKey(symbolAfterDot))
                    {
                        newKernels.Add(symbolAfterDot, new HashSet<LRKernelItem>());
                    }

                    if (!newKernels[symbolAfterDot].ToList().Contains(newItem))
                    {
                        newKernels[symbolAfterDot].Add(newItem);
                    }
                }
            }

            var kernels = Kernels.ToList();
            foreach (string key in kernel.Keys)
            {
                var newKernel = new LRKernel(Kernels.Count, newKernels[key].ToList());

                var targetKernelIndex = kernels.FindIndex(newKernel.Equals);
                if (targetKernelIndex < 0)
                {
                    Kernels.Add(newKernel);
                    targetKernelIndex = newKernel.Index;
                }
                else
                {
                    foreach (var item in newKernel.Items)
                    {
                        var targetItems = kernels[targetKernelIndex].Items;
                        lookAheadsPropagated |= !targetItems.Contains(item);
                        if (!targetItems.Contains(item))
                        {
                            kernels[targetKernelIndex].Items.Add(item);
                        }
                    }
                }

                kernel.Gotos[key] = targetKernelIndex;
                kernels = Kernels.ToList();
            }

            return lookAheadsPropagated;
        }
    }
}