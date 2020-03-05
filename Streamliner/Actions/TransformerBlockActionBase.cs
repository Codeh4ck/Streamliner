using System.Threading;

namespace Streamliner.Actions
{
    public abstract class TransformerBlockActionBase<TIn, TOut> : BlockActionBase
    {
        public abstract bool TryTransform(TIn input, out TOut model, CancellationToken token = default(CancellationToken));
    }
}
