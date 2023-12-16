using System;
using System.Threading;
using System.Threading.Tasks;
using Streamliner.Blocks.Base;
using Streamliner.Definitions;

namespace Streamliner.Core.Links
{
    internal abstract class BlockLinkBase<T> : IBlockLink<T>
    {
        public ITargetBlock<T> TargetBlock { get; }
        public ISourceBlock<T> SourceBlock { get; set; }
        public Func<T, bool> FuncFilter { get; }

        protected abstract void Enqueue(T item, CancellationToken token = default);
        protected abstract Task DelayedEnqueue(T item, TimeSpan delay, CancellationToken token = default);

        protected BlockLinkBase(ISourceBlock<T> sourceBlock, ITargetBlock<T> targetBlock, FlowLinkDefinition<T> linkDefinition)
        {
            FuncFilter = linkDefinition.FuncFilter;
            SourceBlock = sourceBlock;
            TargetBlock = targetBlock;
        }

        public virtual bool TryEnqueue(T item, CancellationToken token = default)
        {
            if (FuncFilter == null || FuncFilter(item))
            {
                Enqueue(item, token);
                return true;
            }

            return false;
        }
        public async Task<bool> TryDelayedEnqueue(T item, TimeSpan delay, CancellationToken token = default)
        {
            if (FuncFilter == null || FuncFilter(item))
            {
                await DelayedEnqueue(item, delay, token);
                return true;
            }

            return false;
        }
    }
}