using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Fluent.Transformer;

namespace Streamliner.Fluent.Factories
{
    public static class TransformerDefinitionFactory
    {
        public static FluentTransformerDefinition CreateDispatcher() =>
            new FluentTransformerDefinition(ProducerType.Dispatcher);
        public static FluentTransformerDefinition CreateBroadcaster() =>
            new FluentTransformerDefinition(ProducerType.Broadcaster);
    }
}