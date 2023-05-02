namespace Streamliner.Core.Links.Remote.MessageQueues
{
    public interface IMqFactory
    {
        IMqConsumer<T> CreateConsumer<T>(string queueName);
        IMqProducer CreateProducer(string exchangeName);
        
        void PurgeQueue(string queueName);
        void BindQueue(BindQueueArgs args);
        void CreateQueue(CreateQueueArgs args);
        void CreateExchange(CreateExchangeArgs args);
    }
}