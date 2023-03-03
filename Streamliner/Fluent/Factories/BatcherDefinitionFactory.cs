using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Fluent.Batcher;

namespace Streamliner.Fluent.Factories;

public static class BatcherDefinitionFactory
{
    public static FluentBatcherDefinition CreateDispatcher() => new(ProducerType.Dispatcher);
    public static FluentBatcherDefinition CreateBroadcaster() => new(ProducerType.Broadcaster);
}