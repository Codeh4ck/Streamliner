namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowTargetSettings : FlowSettings
    {
        public FlowTargetSettings(int capacity, object context = null, uint parallelismInstances = 1) 
            : base(context, parallelismInstances)
        {
            Capacity = capacity;
        }

        public int Capacity { get; }
    }
}