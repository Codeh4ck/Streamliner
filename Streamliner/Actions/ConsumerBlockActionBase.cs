using System.Threading;

namespace Streamliner.Actions
{
    public abstract class ConsumerBlockActionBase<TIn> : BlockActionBase
    {
        public abstract void Consume(TIn model, CancellationToken token = default);
    }
}