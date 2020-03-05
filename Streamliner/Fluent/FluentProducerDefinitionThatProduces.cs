using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent
{
    public class FluentProducerDefinitionThatProduces
    {
        private readonly BlockInfo _blockInfo;
        private readonly FlowProducerSettings _producerSettings;

        internal FluentProducerDefinitionThatProduces(BlockInfo blockInfo, FlowProducerSettings producerSettings)
        {
            _blockInfo = blockInfo;
            _producerSettings = producerSettings;
        }

        public FluentProducerDefinitionWithAction<T> ThatProduces<T>()
        {
            return new FluentProducerDefinitionWithAction<T>(_blockInfo, _producerSettings);
        }

    }
}
