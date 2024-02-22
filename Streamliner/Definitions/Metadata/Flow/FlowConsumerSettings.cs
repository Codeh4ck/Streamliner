namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowConsumerSettings : FlowTargetSettings
    {
        public FlowConsumerSettings(int capacity, object context = null, uint maxDegreeOfParallelism = 1) 
            : base(capacity, context, maxDegreeOfParallelism)
        {  }
    }
}