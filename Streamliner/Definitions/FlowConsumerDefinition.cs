using System;
using System.Collections.Generic;
using Streamliner.Core.Base;
using Streamliner.Definitions.Base;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions;

public class FlowConsumerDefinition<T> : FlowDefinitionItemBase, IFlowTargetDefinition<T>
{
    public FlowTargetSettings Settings { get; }
    public ICollection<FlowLinkDefinition> InboundLinks { get; }

    internal FlowConsumerDefinition(BlockInfo blockInfo, FlowConsumerSettings settings, Type actionType) : base(blockInfo, actionType, BlockType.Consumer)
    {
        Settings = settings;
        InboundLinks = new List<FlowLinkDefinition>();
    }

    public void LinkFrom(FlowLinkDefinition<T> link)
    {
        InboundLinks.Add(link);
    }

    public void GenerateFlowPlanItem(IFlowSourceDefinition<T> parent, IFlowPlan plan, FlowLinkDefinition<T> link)
    {
        plan.AddConsumer(parent.BlockInfo.Id, this, link);
    }
}