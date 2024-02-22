using System;
using System.Threading.Tasks;
using Streamliner.Core.Links;

namespace Streamliner.Core.Routing
{
    public class LinkBroadcasterRouter<T> : LinkRouterBase<T>
    {
        public override void Route(T item)
        {
            foreach (IBlockLink<T> link in Links)
                link.TryEnqueue(item);
        }

        public override async Task DelayedRoute(T item, TimeSpan delay)
        {
            foreach (IBlockLink<T> link in Links)
                await link.TryDelayedEnqueue(item, delay);
        }
    }
}