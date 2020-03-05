using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Fluent.Factories
{
    public static class ProducerDefinitionFactory
    {
        public static FluentProducerDefinition CreateDispatcher() => new FluentProducerDefinition(ProducerType.Dispatcher);
        public static FluentProducerDefinition CreateBroadcaster() => new FluentProducerDefinition(ProducerType.Broadcaster);
    }
}
