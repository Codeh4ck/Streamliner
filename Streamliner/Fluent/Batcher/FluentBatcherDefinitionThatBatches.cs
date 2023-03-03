using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Batcher;

public class FluentBatcherDefinitionThatBatches
{
    private readonly BlockInfo _blockInfo;
    private readonly FlowBatcherSettings _batcherSettings;

    internal FluentBatcherDefinitionThatBatches(BlockInfo blockInfo, FlowBatcherSettings batcherSettings)
    {
        _blockInfo = blockInfo;
        _batcherSettings = batcherSettings;
    }

    public FlowBatcherDefinition<T> ThatBatches<T>() => new(_blockInfo, _batcherSettings);
}