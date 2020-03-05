using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Fluent.Factories
{
    public static class WaiterDefinitionFactory
    {
        public static FluentWaiterDefinition CreateDispatcher() => new FluentWaiterDefinition(ProducerType.Dispatcher);
        public static FluentWaiterDefinition CreateBroadcaster() => new FluentWaiterDefinition(ProducerType.Broadcaster);
    }
}
