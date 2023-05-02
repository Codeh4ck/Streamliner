using System;
using Streamliner.Definitions;
using Streamliner.Blocks.Base;
using Streamliner.Core.Links.Remote.MessageQueues;

namespace Streamliner.Core.Links.Remote
{
    public class RemoteMqLinkFactory : IBlockLinkFactory
    {

        private static IBlockLinkFactory _instance;

        public static IBlockLinkFactory GetInstance(IMqFactory mqFactory) => _instance ?? (_instance = new RemoteMqLinkFactory(mqFactory));

        private const string RoutingKey = "streamliner-remote-links";
        
        private readonly IMqProducer _producer;
        private readonly IMqFactory _mqFactory;

        private RemoteMqLinkFactory(IMqFactory mqFactory)
        {
            _mqFactory = mqFactory ?? throw new ArgumentNullException(nameof(mqFactory));
            _producer = _mqFactory.CreateProducer(RoutingKey);
        }

        public IBlockLink<T> CreateLink<T>(ISourceBlock<T> from, ITargetBlock<T> to, FlowLinkDefinition<T> link) => 
            new RemoteMqLink<T>(_producer, from, to, link);

        public IBlockLinkReceiver<T> CreateReceiver<T>(FlowDefinition flowDefinition, FlowLinkDefinition<T> linkDefinition)
        {
            string routingKey = $"{flowDefinition.ServiceInfo.Id}#{linkDefinition.Target.BlockInfo.Id}";
            
            _mqFactory.CreateQueue(new CreateQueueArgs()
            {
                QueueName = routingKey,
                Durable = true,
                AutoDelete = false
            });
            
            _mqFactory.BindQueue(new BindQueueArgs()
            {
                Queue = routingKey,
                Exchange = RoutingKey,
                BindingKey = routingKey
            });

            IMqConsumer<T> consumer = _mqFactory.CreateConsumer<T>(routingKey);
            return new RemoteMqLinkReceiver<T>(consumer);
        }
    }
}