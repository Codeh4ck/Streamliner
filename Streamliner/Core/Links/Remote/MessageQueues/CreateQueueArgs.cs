namespace Streamliner.Core.Links.Remote.MessageQueues
{
    public class CreateQueueArgs
    {
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public string QueueName { get; set; }
        public long? MessageTtl { get; set; }
    }
}