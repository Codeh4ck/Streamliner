using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Producer;

public class FluentProducerDefinition
{
    private readonly ProducerType _producerType;
    private object _withContext = null;
    private uint _parallelismInstances = 1;

    internal FluentProducerDefinition(ProducerType producerType) => _producerType = producerType;

    public FluentProducerDefinition WithContext(object settings)
    {
        _withContext = settings;
        return this;
    }

    public FluentProducerDefinition WithParallelismInstances(uint instances)
    {
        _parallelismInstances = instances;
        return this;
    }

    public FluentProducerDefinitionThatProduces WithBlockInfo(Guid id, string name) =>
        new(new(id, name, BlockType.Producer),
            new(_producerType, _withContext, _parallelismInstances));
}