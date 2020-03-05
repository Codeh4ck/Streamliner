using Streamliner.Blocks.Base;
using Streamliner.Definitions;

namespace Streamliner.Core.Links
{
    public interface IBlockLinkFactory
    {
        IBlockLink<T> CreateLink<T>(ISourceBlock<T> from, ITargetBlock<T> to, FlowLinkDefinition<T> link);
        IBlockLinkReceiver<T> CreateReceiver<T>(FlowLinkDefinition<T> linkDefinition);
    }
}
