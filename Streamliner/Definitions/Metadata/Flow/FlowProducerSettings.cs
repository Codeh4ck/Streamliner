using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowProducerSettings : FlowSettings
    {
        public ProducerType ProducerType { get; }
        public FlowProducerSettings(ProducerType producerType, object context = null, bool enableAuditing = false, uint parallelismInstances = 1) 
            : base(context, enableAuditing, parallelismInstances)
        {
            ProducerType = producerType;
        }
    }
}
