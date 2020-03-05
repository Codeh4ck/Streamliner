using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Fluent.Factories
{
    public static class TransformerDefinitionFactory
    {
        public static FluentTransformerDefinition CreateDispatcher() => new FluentTransformerDefinition(ProducerType.Dispatcher);
        public static FluentTransformerDefinition CreateBroadcaster() => new FluentTransformerDefinition(ProducerType.Broadcaster);
    }
}
