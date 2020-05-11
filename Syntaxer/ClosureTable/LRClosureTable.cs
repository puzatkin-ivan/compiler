using System.Collections.Generic;
using System.Linq;

namespace compiler.Syntaxer
{
    public class LRClosureTable
    {
        private Grammar _grammar;
        private HashSet<LRKernel> _kernels;

        public LRClosureTable(Grammar grammar)
        {
            _grammar = grammar;
            _kernels = new HashSet<LRKernel>();

            var firstKernelItem = new LRKernelItem(_grammar, grammar.Rules.First(), 0);
            _kernels.Add(new LRKernel(0, new List<LRKernelItem>(){ firstKernelItem }));

            for (int index = 0; index < _kernels.Count;)
            {
                var kernelList = _kernels.ToList();
                var kernel = kernelList[index];

                UpdateClosure(kernel);

                index = AddGotos(kernel) ? 0 : index + 1;
            }
        }

        private void UpdateClosure(LRKernel kernel)
        {
            foreach (LRKernelItem closure in kernel.Closure)
            {
                List<LRKernelItem> newItemsFromSymbolAfterDot = closure.NewItemsFromSymbolAfterDot();

                foreach (LRKernelItem item in newItemsFromSymbolAfterDot)
                {
                    int index = kernel.Closure.FindIndex(lhs => lhs.Equals(item));
                    if (index < 0)
                    {
                        kernel.Closure.Add(item);
                    }
                }
            }
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

                    int keyIndex = kernel.Keys.FindIndex(lhs => lhs.Equals(symbolAfterDot));
                    if (keyIndex < 0)
                    {
                        kernel.Keys.Add(symbolAfterDot);   
                    }

                    if (!newKernels.ContainsKey(symbolAfterDot))
                    {
                        newKernels.Add(symbolAfterDot, new HashSet<LRKernelItem>());
                    }
                    newKernels[symbolAfterDot].Add(newItem);
                }
            }

            var kernels = _kernels.ToList();
            foreach (string key in kernel.Keys)
            {
                var newKernel = new LRKernel(_kernels.Count, newKernels[key].ToList());
                var targetKernelIndex = kernels.FindIndex(lhs => lhs.Equals(newKernel));

                if (targetKernelIndex < 0)
                {
                    _kernels.Add(newKernel);
                    kernels = _kernels.ToList();
                    targetKernelIndex = newKernel.Index;
                }
                else
                {
                    foreach (var item in newKernel.Items)
                    {
                        var targetItems = kernels[targetKernelIndex].Items;
                        if (!targetItems.Contains(item))
                        {
                            targetItems.Add(item);
                        }

                        lookAheadsPropagated |= false;
                    }
                }

                kernel.Gotos[key] = targetKernelIndex;
            }

            return lookAheadsPropagated;
        }
    }
}