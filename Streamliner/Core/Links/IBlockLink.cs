using System;
using System.Threading;
using Streamliner.Blocks.Base;

namespace Streamliner.Core.Links
{
    public interface IBlockLink<T>
    {
        bool TryEnqueue(T item, CancellationToken token = default(CancellationToken));
        bool TryDelayedEnqueue(T item, TimeSpan delay, CancellationToken token = default(CancellationToken));
        ITargetBlock<T> TargetBlock { get; }
        ISourceBlock<T> SourceBlock { get; set; }
    }
}
