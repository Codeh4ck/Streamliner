using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Streamliner.Core.Links.Remote.MessageQueues
{
    public interface IMqProducer : IDisposable
    {
        void Produce<T>(T model, string queueName = null, CancellationToken token = default);
        void Produce<T>(IEnumerable<T> models, string queueName = null, CancellationToken token = default);
        Task ProduceAsync<T>(T model, string queueName = null, CancellationToken token = default);
        Task ProduceAsync<T>(IEnumerable<T> models, string queueName = null, CancellationToken token = default);
    }
}