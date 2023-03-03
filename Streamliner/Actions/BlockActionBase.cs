using Streamliner.Core.Utilities;
using Streamliner.Core.Utilities.Auditing;
using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Actions;

public abstract class BlockActionBase : Runnable
{
    public object Context { get; set; }
    public ILogger Logger { get; set; }
    public BlockHeader Header { get; set; }

    protected override void OnStart(object context = null) { }
    protected override void OnStop() { }
}