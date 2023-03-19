using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Streamliner.Core.Links.Local
{
    internal class LocalLinkReceiver<T> : IBlockLinkReceiver<T>
    {
        private readonly BlockingCollection<T> _buffer;

        public LocalLinkReceiver(BlockingCollection<T> buffer) 
            => _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));

        public T Receive(CancellationToken token = default) => _buffer.Take(token);
    }
}