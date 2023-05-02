namespace Streamliner.Core.Links.Remote.MessageQueues
{
    public class InitQueueArgs
    {
        public BindQueueArgs BindQueueArgs { get; set; }
        public CreateQueueArgs CreateQueueArgs { get; set; }
        public CreateExchangeArgs CreateExchangeArgs { get; set; }
    }
}