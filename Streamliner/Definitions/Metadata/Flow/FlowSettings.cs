namespace Streamliner.Definitions.Metadata.Flow;

public abstract class FlowSettings
{
    protected FlowSettings(object context = null, uint parallelismInstances = 1, uint iterations = 1)
    {
        Context = context;
        ParallelismInstances = parallelismInstances;
        Iterations = iterations;
    }
    public FlowType Type { get; set; }
    public uint Iterations { get; set; }
    public object Context { get; set; }
    public uint ParallelismInstances { get; set; }
}