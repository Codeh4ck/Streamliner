using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Streamliner.Core.Links;

namespace Streamliner.Core.Routing
{
    public abstract class LinkRouterBase<T>
    {
        protected readonly List<IBlockLink<T>> Links = new List<IBlockLink<T>>();

        public void AddLink(IBlockLink<T> link) => Links.Add(link);

        public abstract void Route(T item);
        public abstract Task DelayedRoute(T item, TimeSpan delay);
    }
}