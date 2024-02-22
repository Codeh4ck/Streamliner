using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Fluent.Batcher;
using Streamliner.Fluent.Partitioner;

namespace Streamliner.Fluent.Factories
{
    public static class PartitionerDefinitionFactory
    {
        public static FluentPartitionerDefinition CreateDispatcher() => new FluentPartitionerDefinition(ProducerType.Dispatcher);
        public static FluentPartitionerDefinition CreateBroadcaster() => new FluentPartitionerDefinition(ProducerType.Broadcaster);
    }
}