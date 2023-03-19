using System.Threading;

namespace Streamliner.Core.Links
{
    public interface IBlockLinkReceiver<T>
    {
        T Receive(CancellationToken token = default);
    }
}