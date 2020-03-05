using Streamliner.Actions;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent
{
    public class FluentConsumerDefinitionWithAction<T>
    {
        private readonly BlockInfo _blockInfo;
        private readonly FlowConsumerSettings _consumerSettings;

        public FluentConsumerDefinitionWithAction(BlockInfo blockInfo, FlowConsumerSettings consumerSettings)
        {
            _blockInfo = blockInfo;
            _consumerSettings = consumerSettings;
        }

        public FlowConsumerDefinition<T> WithAction<TAction>() where TAction : ConsumerBlockActionBase<T>
        {
            return new FlowConsumerDefinition<T>(_blockInfo, _consumerSettings, typeof(TAction));
        }
    }
}
