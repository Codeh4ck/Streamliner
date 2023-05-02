namespace Streamliner.Core.Links.Remote.MessageQueues
{
    public class BindQueueArgs
    {
        public string Queue { get; set; }
        public string Exchange { get; set; }
        public string BindingKey { get; set; }
    }
}