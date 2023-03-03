using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Fluent.Producer;

namespace Streamliner.Fluent.Factories;

public static class ProducerDefinitionFactory
{
    public static FluentProducerDefinition CreateDispatcher() => new(ProducerType.Dispatcher);
    public static FluentProducerDefinition CreateBroadcaster() => new(ProducerType.Broadcaster);
}