using System.Threading;

namespace Streamliner.Actions;

public abstract class ProducerBlockActionBase<T> : BlockActionBase
{
    public abstract bool TryProduce(out T model, CancellationToken token = default);
}