using System;
using System.Threading;
using Streamliner.Actions;
using Streamliner.Core.Links.Remote.MessageQueues;

namespace Streamliner.Utilities
{
    public abstract class SendToMqConsumerAction<T> : ConsumerBlockActionBase<T>
    {
        private IMqProducer _producer;
        private readonly IMqFactory _mqFactory;

        private readonly string _routingKey;
        private readonly string _exchangeName;
        protected abstract CreateExchangeArgs ExchangeArgs { get; }

        protected SendToMqConsumerAction(string exchangeName, IMqFactory mqFactory)
        {
            _mqFactory = mqFactory ?? throw new ArgumentNullException(nameof(mqFactory));
            _exchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
        }

        protected SendToMqConsumerAction(string routingKey, string exchangeName, IMqFactory mqFactory) : this(
            exchangeName, mqFactory)
            => _routingKey = routingKey ?? throw new ArgumentNullException(nameof(routingKey));

        public override void Consume(T model, CancellationToken token = default) =>
            _producer.Produce<T>(model, _routingKey, token);

        protected override void OnStart(object context = null)
        {
            base.OnStart(context);
            _mqFactory.CreateExchange(ExchangeArgs);
            _producer = _mqFactory.CreateProducer(_exchangeName);
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (_producer == null) return;

            _producer.Dispose();
            _producer = null;
        }
    }
}