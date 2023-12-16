using System.Threading;
using System.Threading.Tasks;

namespace Streamliner.Actions
{
    public abstract class ProducerBlockActionBase<T> : BlockActionBase
    {
        public abstract Task<BlockActionResult<T>> TryProduce(CancellationToken token = default);
    }
}