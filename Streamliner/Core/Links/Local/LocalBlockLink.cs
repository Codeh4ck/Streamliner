using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Streamliner.Blocks.Base;
using Streamliner.Definitions;

namespace Streamliner.Core.Links.Local
{
    internal class LocalBlockLink<T> : BlockLinkBase<T>
    {
        private readonly BlockingCollection<T> _buffer;

        public LocalBlockLink(BlockingCollection<T> buffer, ISourceBlock<T> sourceBlock, ITargetBlock<T> targetBlock,
            FlowLinkDefinition<T> linkDefinition) 
            : base(sourceBlock, targetBlock, linkDefinition) =>
            _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));

        protected override void Enqueue(T item, CancellationToken token = default) => _buffer.Add(item, token);

        protected override void DelayedEnqueue(T item, TimeSpan delay, CancellationToken token = default) =>
            Task.Delay(delay, token).ContinueWith(_ => Enqueue(item, token), token);
    }
}