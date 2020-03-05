using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Streamliner.Core.Links.Local
{
    internal class LocalLinkReceiver<T> : IBlockLinkReceiver<T>
    {
        private readonly BlockingCollection<T> _transport;

        public LocalLinkReceiver(BlockingCollection<T> transport)
        {
            if (transport == null) throw new ArgumentNullException(nameof(transport));
            _transport = transport;
        }
        public T Receive(CancellationToken token = default(CancellationToken))
        {
            return _transport.Take(token);
        }

        public void Accept(T t, CancellationToken token = default(CancellationToken)) { }
    }
}
