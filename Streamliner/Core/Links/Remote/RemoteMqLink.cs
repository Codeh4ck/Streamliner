using System;
using System.Threading;
using System.Threading.Tasks;
using Streamliner.Definitions;
using Streamliner.Blocks.Base;
using Streamliner.Core.Links.Remote.MessageQueues;

namespace Streamliner.Core.Links.Remote
{
    internal class RemoteMqLink<T> : BlockLinkBase<T>
    {
        private readonly IMqProducer _mqProducer;
        private readonly string _routingKey;
        
        public RemoteMqLink(
            IMqProducer mqProducer,
            ISourceBlock<T> sourceBlock, 
            ITargetBlock<T> targetBlock, 
            FlowLinkDefinition<T> linkDefinition) : base(sourceBlock, targetBlock, linkDefinition)
        {
            _mqProducer = mqProducer ?? throw new ArgumentNullException(nameof(mqProducer));
            _routingKey = $"{TargetBlock.Header.ServiceInfo.Id}#{TargetBlock.Header.BlockInfo.Id}";
        }

        protected override void Enqueue(T item, CancellationToken token = default) => _mqProducer.Produce(item, _routingKey, token);

        protected override void DelayedEnqueue(T item, TimeSpan delay, CancellationToken token = default) => 
            Task.Delay(delay, token).ContinueWith(_ => Enqueue(item, token), token);
    }
}