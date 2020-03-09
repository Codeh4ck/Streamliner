using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Fluent.Waiter;

namespace Streamliner.Fluent.Factories
{
    public static class WaiterDefinitionFactory
    {
        public static FluentWaiterDefinition CreateDispatcher() => new FluentWaiterDefinition(ProducerType.Dispatcher);
        public static FluentWaiterDefinition CreateBroadcaster() => new FluentWaiterDefinition(ProducerType.Broadcaster);
    }
}
