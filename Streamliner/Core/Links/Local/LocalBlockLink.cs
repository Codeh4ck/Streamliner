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
            : base(sourceBlock, targetBlock, linkDefinition)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            _buffer = buffer;
        }

        protected override void Enqueue(T item, CancellationToken token = default(CancellationToken))
        {
            _buffer.Add(item, token);
        }

        protected override void DelayedEnqueue(T item, TimeSpan delay, CancellationToken token = default(CancellationToken))
        {
            Task.Delay(delay, token).ContinueWith(_ => Enqueue(item, token), token);
        }
    }
}
