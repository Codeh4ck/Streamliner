using System.Threading;
using System.Threading.Tasks;
using Streamliner.Actions;
using Streamliner.Core.Routing;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Blocks
{
    public sealed class ProducerBlock<T> : SourceBlockBase<T>
    {
        private readonly ProducerBlockActionBase<T> _action;

        public ProducerBlock(
            BlockHeader header,
            LinkRouterBase<T> router,
            ProducerBlockActionBase<T> action,
            FlowProducerDefinition<T> definition) : base(header, definition.Settings, router)
        {
            _action = action;
        }

        protected override async Task ProcessItem(CancellationToken token = default)
        {
            var result = await _action.TryProduce(token);
            
            if (result.Continue)
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