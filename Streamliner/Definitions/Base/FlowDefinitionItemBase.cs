using System;
using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Definitions.Base;

public abstract class FlowDefinitionItemBase : IFlowDefinition
{
    public BlockInfo BlockInfo { get; }
    public Type ActionType { get; }
    public BlockType BlockType { get; }

    protected FlowDefinitionItemBase(BlockInfo blockInfo, Type actionType, BlockType blockType)
    {
        BlockInfo = blockInfo;
        ActionType = actionType;
        BlockType = blockType;
    }
}