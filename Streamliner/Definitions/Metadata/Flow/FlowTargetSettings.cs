namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowTargetSettings : FlowSettings
    {
        public FlowTargetSettings(int capacity, object context = null, uint maxDegreeOfParallelism = 1) 
            : base(context, maxDegreeOfParallelism)
        {
            Capacity = capacity;
        }

        public int Capacity { get; }
    }
}