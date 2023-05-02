using System;
using System.Threading;
using System.Threading.Tasks;

namespace Streamliner.Core.Links.Remote.MessageQueues
{
    public interface IMqConsumer<T> : IDisposable
    {
        T Consume(CancellationToken token = default);
        Task<T> ConsumeAsync(CancellationToken token = default);
    }
}