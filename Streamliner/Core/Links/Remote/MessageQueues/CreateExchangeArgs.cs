namespace Streamliner.Core.Links.Remote.MessageQueues
{
    public class CreateExchangeArgs
    {
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
    }
}