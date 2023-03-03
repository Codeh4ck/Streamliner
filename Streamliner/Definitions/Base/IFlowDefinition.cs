using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Definitions.Base;

public interface IFlowDefinition
{
    BlockInfo BlockInfo { get; }
}