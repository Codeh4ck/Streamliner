namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowConsumerSettings : FlowTargetSettings
    {
        public FlowConsumerSettings(int capacity, object context = null, uint parallelismInstances = 1) 
            : base(capacity, context, parallelismInstances)
        {  }
    }
}