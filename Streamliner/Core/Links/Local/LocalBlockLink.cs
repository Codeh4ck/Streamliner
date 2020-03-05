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
        private readonly BlockingCollection<T> _transport;

        public LocalBlockLink(BlockingCollection<T> transport, ISourceBlock<T> sourceBlock, ITargetBlock<T> targetBlock,
            FlowLinkDefinition<T> linkDefinition) 
            : base(sourceBlock, targetBlock, linkDefinition)
        {
            if (transport == null) throw new ArgumentNullException(nameof(transport));
            _transport = transport;
        }

        protected override void Enqueue(T item, CancellationToken token = default(CancellationToken))
        {
            _transport.Add(item, token);
        }

        protected override void DelayedEnqueue(T item, TimeSpan delay, CancellationToken token = default(CancellationToken))
        {
            Task.Delay(delay, token).ContinueWith(_ => Enqueue(item, token), token);
        }
    }
}
