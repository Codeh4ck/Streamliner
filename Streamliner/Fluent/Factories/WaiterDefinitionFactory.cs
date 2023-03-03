using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Fluent.Waiter;

namespace Streamliner.Fluent.Factories;

public static class WaiterDefinitionFactory
{
    public static FluentWaiterDefinition CreateDispatcher() => new(ProducerType.Dispatcher);
    public static FluentWaiterDefinition CreateBroadcaster() => new(ProducerType.Broadcaster);
}