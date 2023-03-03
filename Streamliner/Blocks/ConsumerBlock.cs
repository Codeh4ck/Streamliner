using System.Threading;
using Streamliner.Actions;
using Streamliner.Blocks.Base;
using Streamliner.Core.Links;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Blocks;

public sealed class ConsumerBlock<T> : BlockBase, ITargetBlock<T>
{
    public IBlockLinkReceiver<T> Receiver { get; }
    private readonly ConsumerBlockActionBase<T> _action;

    public ConsumerBlock(BlockHeader header, IBlockLinkReceiver<T> receiver, ConsumerBlockActionBase<T> action, FlowConsumerDefinition<T> definition) :
        base(header, definition.Settings)
    {
        Receiver = receiver;
        _action = action;
    }

    protected override void ProcessItem(CancellationToken token = default)
    {
        T item = Receiver.Receive(token);
        _action.Consume(item, token);
    }

    protected override void OnStart(object context = null)
    {
        _action.Start(context);
        base.OnStart(context);
    }

    protected override void OnStop()
    {
        _action.Stop();
        base.OnStop();
    }
}