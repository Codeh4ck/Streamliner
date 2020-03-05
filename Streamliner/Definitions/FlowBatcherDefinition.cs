using System.Collections.Generic;
using Streamliner.Core.Base;
using Streamliner.Definitions.Base;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions
{
    public class FlowBatcherDefinition<T> : FlowTransformerDefinitionBase<T, List<T>>
    {
        internal FlowBatcherDefinition(BlockInfo blockInfo, FlowTargetSettings settings) : base(blockInfo, settings, null, BlockType.Batcher)
        {
        }

        public override void GenerateFlowPlanItem(IFlowSourceDefinition<T> parent, IFlowPlan plan, FlowLinkDefinition<T> link)
        {
            plan.AddBatcher(parent.BlockInfo.Id, this, link);

            foreach(FlowLinkDefinition<List<T>> outboundLink in OutboundLinks)
                outboundLink.Target.GenerateFlowPlanItem(this, plan, outboundLink);
        }
    }
}
