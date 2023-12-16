using System.Threading;
using System.Threading.Tasks;

namespace Streamliner.Actions
{
    public abstract class ConsumerBlockActionBase<TIn> : BlockActionBase
    {
        public abstract Task Consume(TIn model, CancellationToken token = default);
    }
}