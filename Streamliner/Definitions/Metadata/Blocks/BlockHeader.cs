using Streamliner.Core.Base;

namespace Streamliner.Definitions.Metadata.Blocks
{
    public sealed class BlockHeader
    {
        public BlockInfo BlockInfo { get; set; }
        public ServiceInfo ServiceInfo { get; set; }
        public BlockHeader(BlockInfo blockInfo, ServiceInfo serviceInfo)
        {
            BlockInfo = blockInfo;
            ServiceInfo = serviceInfo;
        }
    }
}
