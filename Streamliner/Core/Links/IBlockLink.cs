using System;
using System.Threading;
using System.Threading.Tasks;
using Streamliner.Blocks.Base;

namespace Streamliner.Core.Links
{
    public interface IBlockLink<T>
    {
        bool TryEnqueue(T item, CancellationToken token = default);
        Task<bool> TryDelayedEnqueue(T item, TimeSpan delay, CancellationToken token = default);
        ITargetBlock<T> TargetBlock { get; }
        ISourceBlock<T> SourceBlock { get; set; }
    }
}