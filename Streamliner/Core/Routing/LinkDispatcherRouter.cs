using System;
using System.Threading;
using System.Threading.Tasks;

namespace Streamliner.Core.Routing
{
    public class LinkDispatcherRouter<T> : LinkRouterBase<T>
    {
        private int _startIndex = 0;

        public override void Route(T item)
        {
            if (Links.Count == 1)
            {
                Links[0].TryEnqueue(item);
                return;
            }

            for (int x = _startIndex; x < Links.Count + _startIndex; x++)
            {
                if (Links[x % Links.Count].TryEnqueue(item))
                {
                    Interlocked.Increment(ref _startIndex);
                    return;
                }
            }
        }

        public override async Task DelayedRoute(T item, TimeSpan delay)
        {
            if (Links.Count == 1)
            {
                await Links[0].TryDelayedEnqueue(item, delay);
                return;
            }

            for (int x = _startIndex; x < Links.Count + _startIndex; x++)
            {
                if (await Links[x % Links.Count].TryDelayedEnqueue(item, delay))
                {
                    Interlocked.Increment(ref _startIndex);
                    return;
                }
            }
        }
    }
}