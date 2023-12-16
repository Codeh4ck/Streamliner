using System.Threading;
using System.Threading.Tasks;

namespace Streamliner.Actions
{
    public abstract class ProducerBlockActionBase<T> : BlockActionBase
    {
        public abstract Task<bool> TryProduce(out T model, CancellationToken token = default);
    }
}