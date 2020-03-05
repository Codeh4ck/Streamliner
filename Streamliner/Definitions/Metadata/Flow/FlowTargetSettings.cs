namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowTargetSettings : FlowSettings
    {
        public FlowTargetSettings(int capacity, object context = null, bool enableAuditing = false, uint parallelismInstances = 1) 
            : base(context, enableAuditing, parallelismInstances)
        {
            Capacity = capacity;
        }

        public int Capacity { get; }
    }
}
