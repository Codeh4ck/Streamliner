using System.Threading;
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

        protected override void ProcessItem(CancellationToken token = default(CancellationToken))
        {
            if (_action.TryProduce(out T t, token))
                Router.Route(t);
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
