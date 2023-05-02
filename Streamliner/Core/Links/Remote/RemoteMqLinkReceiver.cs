using System;
using System.Threading;
using Streamliner.Core.Links.Remote.MessageQueues;

namespace Streamliner.Core.Links.Remote
{
    public class RemoteMqLinkReceiver<T> : IBlockLinkReceiver<T>
    {
        private readonly IMqConsumer<T> _consumer;

        public RemoteMqLinkReceiver(IMqConsumer<T> consumer)
        {
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        }
        
        public T Receive(CancellationToken token = default) => _consumer.Consume();
    }
}