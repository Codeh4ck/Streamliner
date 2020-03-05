using System.Collections.Generic;
using Streamliner.Blocks.Base;
using Streamliner.Core.Links;
using Streamliner.Core.Routing;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Blocks
{
    public abstract class SourceBlockBase<T> : BlockBase, ISourceBlock<T>
    {
        public LinkRouterBase<T> Router { get; }
        public List<ITargetBlock<T>> Children { get; }

        protected SourceBlockBase(BlockHeader header, FlowSettings settings, LinkRouterBase<T> router) : base(header, settings)
        {
            Router = router;
            Children = new List<ITargetBlock<T>>();
        }

        public void Trigger(T item)
        {
            Router.Route(item);
        }

        public void AddLink(IBlockLink<T> link)
        {
            Router.AddLink(link);
            Children.Add(link.TargetBlock);
        }

        protected override void OnStart(object context = null)
        {
            foreach (ITargetBlock<T> target in Children)
                target.Start(context);

            base.OnStart(context);
        }

        protected override void OnStop()
        {
            base.OnStop();

            foreach(ITargetBlock<T> target in Children)
                target.Stop();
        }

        public override void Wait()
        {
            foreach (ITargetBlock<T> target in Children)
                target.Wait();

            base.Wait();
        }
    }
}
