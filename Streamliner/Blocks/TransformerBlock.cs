using System.Threading;
using System.Threading.Tasks;
using Streamliner.Actions;
using Streamliner.Blocks.Base;
using Streamliner.Core.Links;
using Streamliner.Core.Routing;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Blocks
{
    public sealed class TransformerBlock<TIn, TOut> : SourceBlockBase<TOut>, ITargetBlock<TIn>
    {
        public IBlockLinkReceiver<TIn> Receiver { get; }
        private readonly TransformerBlockActionBase<TIn, TOut> _action;

        public TransformerBlock(BlockHeader header, IBlockLinkReceiver<TIn> receiver, LinkRouterBase<TOut> router, 
            TransformerBlockActionBase<TIn, TOut> action, FlowTransformerDefinition<TIn, TOut> definition) : base(header, definition.Settings, router)
        {
            Receiver = receiver;
            _action = action;
        }

        protected override async Task ProcessItem(CancellationToken token = default)
        {
            TIn tin = Receiver.Receive(token);
            var result = await _action.TryTransform(tin, token);
            
            if (!result.Continue)
                return;

            Router.Route(result.Model);
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
}