using Streamliner.Blocks.Base;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent
{
    public class FluentWaiterDefinitionThatWaits
    {
        private readonly BlockInfo _blockInfo;
        private readonly FlowWaiterSettings _waiterSettings;

        internal FluentWaiterDefinitionThatWaits(BlockInfo blockInfo, FlowWaiterSettings waiterSettings)
        {
            _blockInfo = blockInfo;
            _waiterSettings = waiterSettings;
        }

        public FlowWaiterDefinition<T> ThatWaits<T>() where T : IWaitable
        {
            return new FlowWaiterDefinition<T>(_blockInfo, _waiterSettings);
        }
    }
}
