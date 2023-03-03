using System;
using Streamliner.Core.Links;

namespace Streamliner.Core.Routing;

public class LinkBroadcasterRouter<T> : LinkRouterBase<T>
{
    public override void Route(T item)
    {
        foreach (IBlockLink<T> link in Links)
            link.TryEnqueue(item);
    }

    public override void DelayedRoute(T item, TimeSpan delay)
    {
        foreach (IBlockLink<T> link in Links)
            link.TryDelayedEnqueue(item, delay);
    }
}