using System;
using System.Threading;

namespace Streamliner.Core.Routing
{
    public class LinkDispatcherRouter<T> : LinkRouterBase<T>
    {
        private int _startIndex;

        public LinkDispatcherRouter()
        {
            _startIndex = 0;
        }

        public override void Route(T item)
        {
            if (Links.Count == 1)
            {
                Links[0].TryEnqueue(item);
                return;
            }

            // We need a round-robin approach here to ensure all blocks get queued in a timely manner
            for (int x = _startIndex; x < Links.Count + _startIndex; x++)
            {
                if (Links[x % Links.Count].TryEnqueue(item))
                {
                    Interlocked.Increment(ref _startIndex);
                    return;
                }
            }
        }

        public override void DelayedRoute(T item, TimeSpan delay)
        {
            if (Links.Count == 1)
            {
                Links[0].TryDelayedEnqueue(item, delay);
                return;
            }

            // We need a round-robin approach here to ensure all blocks get queued in a timely manner
            for (int x = _startIndex; x < Links.Count + _startIndex; x++)
            {
                if (Links[x % Links.Count].TryDelayedEnqueue(item, delay))
                {
                    Interlocked.Increment(ref _startIndex);
                    return;
                }
            }
        }
    }
}
