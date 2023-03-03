using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Consumer;

public class FluentConsumerDefinition
{
    private object _withContext = null;
    private int _capacity = 1;
    private uint _parallelismInstances = 1;

    internal FluentConsumerDefinition() { }

    public FluentConsumerDefinition WithContext(object settings)
    {
        _withContext = settings;
        return this;
    }

    public FluentConsumerDefinition WithParallelismInstances(uint instances)
    {
        _parallelismInstances = instances;
        return this;
    }

    public FluentConsumerDefinition WithCapacity(int capacity)
    {
        _capacity = capacity;
        return this;
    }

    public FluentConsumerDefinitionThatConsumes WithBlockInfo(Guid id, string name) =>
        new(new(id, name, BlockType.Producer),
            new(_capacity, _withContext, _parallelismInstances));
}