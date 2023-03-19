using System;
using System.Threading;
using Streamliner.Blocks.Base;

namespace Streamliner.Core.Links
{
    public interface IBlockLink<T>
    {
        bool TryEnqueue(T item, CancellationToken token = default);
        bool TryDelayedEnqueue(T item, TimeSpan delay, CancellationToken token = default);
        ITargetBlock<T> TargetBlock { get; }
        ISourceBlock<T> SourceBlock { get; set; }
    }
}