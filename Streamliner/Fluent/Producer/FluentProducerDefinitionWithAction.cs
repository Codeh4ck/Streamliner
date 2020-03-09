using Streamliner.Actions;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Producer
{
    public class FluentProducerDefinitionWithAction<T>
    {
        private readonly BlockInfo _blockInfo;
        private readonly FlowProducerSettings _producerSettings;

        public FluentProducerDefinitionWithAction(BlockInfo blockInfo, FlowProducerSettings producerSettings)
        {
            _blockInfo = blockInfo;
            _producerSettings = producerSettings;
        }

        public FlowProducerDefinition<T> WithAction<TAction>() where TAction : ProducerBlockActionBase<T>
        {
            return new FlowProducerDefinition<T>(_blockInfo, _producerSettings, typeof(TAction));
        }
    }
}
