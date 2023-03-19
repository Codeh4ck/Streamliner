using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Consumer
{
    public class FluentConsumerDefinitionThatConsumes
    {
        private readonly BlockInfo _blockInfo;
        private readonly FlowConsumerSettings _consumerSettings;

        internal FluentConsumerDefinitionThatConsumes(BlockInfo blockInfo, FlowConsumerSettings consumerSettings)
        {
            _blockInfo = blockInfo;
            _consumerSettings = consumerSettings;
        }

        public FluentConsumerDefinitionWithAction<T> ThatConsumes<T>() =>
            new FluentConsumerDefinitionWithAction<T>(_blockInfo, _consumerSettings);
    }
}