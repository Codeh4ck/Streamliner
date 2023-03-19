using Streamliner.Core.Links;

namespace Streamliner.Blocks.Base
{
    public interface ITargetBlock<T> : IBlock
    {
        IBlockLinkReceiver<T> Receiver { get; }
    }
}