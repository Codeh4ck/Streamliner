using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Streamliner.Core.Links.Local
{
    internal class LocalLinkReceiver<T> : IBlockLinkReceiver<T>
    {
        private readonly BlockingCollection<T> _buffer;

        public LocalLinkReceiver(BlockingCollection<T> buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            _buffer = buffer;
        }
        public T Receive(CancellationToken token = default(CancellationToken))
        {
            return _buffer.Take(token);
        }
    }
}
