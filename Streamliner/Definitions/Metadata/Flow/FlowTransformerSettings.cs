using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowTransformerSettings : FlowTargetSettings
    {
        public ProducerType ProducerType { get; }
        public FlowTransformerSettings(ProducerType producerType, int capacity, object context = null, uint parallelismInstances = 1) : base(capacity, context, parallelismInstances)
        {
            ProducerType = producerType;
        }
    }
}