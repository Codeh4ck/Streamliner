using System;
using Streamliner.Core.Base;
using Streamliner.Definitions.Base;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions
{
    public class FlowTransformerDefinition<TIn, TOut> : FlowTransformerDefinitionBase<TIn, TOut>
    {
        internal FlowTransformerDefinition(BlockInfo blockInfo, FlowTransformerSettings settings, Type actionType) 
            : base(blockInfo, settings, actionType, BlockType.Transformer)
        {
        }

        public override void GenerateFlowPlanItem(IFlowSourceDefinition<TIn> parent, IFlowPlan plan, FlowLinkDefinition<TIn> link)
        {
            plan.AddTransformer(parent.BlockInfo.Id, this, link);

            foreach (FlowLinkDefinition<TOut> outboundLink in OutboundLinks)
                outboundLink.Target.GenerateFlowPlanItem(this, plan, outboundLink);
        }
    }
}