using System.Threading;
using System.Threading.Tasks;

namespace Streamliner.Actions
{
    public abstract class TransformerBlockActionBase<TIn, TOut> : BlockActionBase
    {
        public abstract Task<bool> TryTransform(TIn input, out TOut model, CancellationToken token = default);
    }
}