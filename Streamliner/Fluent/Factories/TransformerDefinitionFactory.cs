using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Fluent.Transformer;

namespace Streamliner.Fluent.Factories;

public static class TransformerDefinitionFactory
{
    public static FluentTransformerDefinition CreateDispatcher() => new(ProducerType.Dispatcher);
    public static FluentTransformerDefinition CreateBroadcaster() => new(ProducerType.Broadcaster);
}