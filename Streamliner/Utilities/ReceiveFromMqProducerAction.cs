using System;
using System.Threading;
using Streamliner.Actions;
using Streamliner.Core.Links.Remote.MessageQueues;

namespace Streamliner.Utilities
{
    public abstract class ReceiveFromMqProducerAction<T> : ProducerBlockActionBase<T>
    {
        private IMqConsumer<T> _consumer;
        private readonly IMqFactory _mqFactory;
        protected abstract InitQueueArgs InitiateQueueArgs { get; }

        protected ReceiveFromMqProducerAction(IMqFactory mqFactory) => 
            _mqFactory = mqFactory ?? throw new ArgumentNullException(nameof(mqFactory));

        public override bool TryProduce(out T model, CancellationToken token = default)
        {
            model = _consumer.Consume(token);
            return true;
        }
        
        protected override void OnStart(object context = null)
        {
            base.OnStart(context);
            
            InitQueueArgs initQueueArgs = InitiateQueueArgs;
            
            _mqFactory.CreateQueue(initQueueArgs.CreateQueueArgs);
            _mqFactory.CreateExchange(initQueueArgs.CreateExchangeArgs);
            _mqFactory.BindQueue(initQueueArgs.BindQueueArgs);
            
            _consumer = _mqFactory.CreateConsumer<T>(initQueueArgs.CreateQueueArgs.QueueName);
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (_consumer == null) return;
            
            _consumer.Dispose();
            _consumer = null;
        }
    }
}