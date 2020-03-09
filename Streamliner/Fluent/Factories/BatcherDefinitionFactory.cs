using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Fluent.Batcher;

namespace Streamliner.Fluent.Factories
{
    public static class BatcherDefinitionFactory
    {
        public static FluentBatcherDefinition CreateDispatcher() => new FluentBatcherDefinition(ProducerType.Dispatcher);
        public static FluentBatcherDefinition CreateBroadcaster() => new FluentBatcherDefinition(ProducerType.Broadcaster);
    }
}
