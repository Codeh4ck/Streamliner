using System.Threading;
using Streamliner.Blocks.Base;
using Streamliner.Core.Links;
using Streamliner.Core.Routing;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Blocks
{
    public sealed class WaiterBlock<T> : SourceBlockBase<T>, ITargetBlock<T> where T : IWaitable
    {
        public IBlockLinkReceiver<T> Receiver { get; }
        public WaiterBlock(BlockHeader header, IBlockLinkReceiver<T> receiver, LinkRouterBase<T> router, FlowWaiterDefinition<T> definition) 
            : base(header, definition.Settings, router)
        {
            Receiver = receiver;
        }

        protected override void ProcessItem(CancellationToken token = default(CancellationToken))
        {
            T item = Receiver.Receive(token);
            Router.DelayedRoute(item, item.WaitFor);
            Receiver.Accept(item, token);
        }
    }
}
