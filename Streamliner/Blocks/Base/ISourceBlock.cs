using Streamliner.Core.Links;

namespace Streamliner.Blocks.Base;

public interface ISourceBlock<T> : IBlock
{
    void Trigger(T item);
    void AddLink(IBlockLink<T> link);
}