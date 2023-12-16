using System.Threading;
using System.Threading.Tasks;

namespace Streamliner.Actions
{
    public abstract class TransformerBlockActionBase<TIn, TOut> : BlockActionBase
    {
        public abstract Task<BlockActionResult<TOut>> TryTransform(TIn input, CancellationToken token = default);
    }
}