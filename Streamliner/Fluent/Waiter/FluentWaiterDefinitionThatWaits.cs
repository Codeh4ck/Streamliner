using Streamliner.Blocks.Base;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Waiter;

public class FluentWaiterDefinitionThatWaits
{
    private readonly BlockInfo _blockInfo;
    private readonly FlowWaiterSettings _waiterSettings;

    internal FluentWaiterDefinitionThatWaits(BlockInfo blockInfo, FlowWaiterSettings waiterSettings)
    {
        _blockInfo = blockInfo;
        _waiterSettings = waiterSettings;
    }

    public FlowWaiterDefinition<T> ThatWaits<T>() where T : IWaitable => 
        new(_blockInfo, _waiterSettings);
}