namespace Streamliner.Definitions.Metadata.Flow
{
    public abstract class FlowSettings
    {
        protected FlowSettings(object context = null, uint maxDegreeOfParallelism = 1, uint iterations = 1)
        {
            Context = context;
            MaxDegreeOfParallelism = maxDegreeOfParallelism;
            Iterations = iterations;
        }
        public FlowType Type { get; set; }
        public uint Iterations { get; set; }
        public object Context { get; set; }
        public uint MaxDegreeOfParallelism { get; set; }
    }
}