using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowWaiterSettings : FlowTargetSettings
    {
        public ProducerType ProducerType { get; }

        public FlowWaiterSettings(ProducerType producerType, int capacity, object context = null, uint maxDegreeOfParallelism = 1) : base(capacity, context, maxDegreeOfParallelism)
        {
            ProducerType = producerType;
        }
    }
}