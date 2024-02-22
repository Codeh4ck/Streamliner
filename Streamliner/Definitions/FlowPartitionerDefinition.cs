using System.Collections.Generic;
using Streamliner.Core.Base;
using Streamliner.Definitions.Base;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions
{
    public class FlowPartitionerDefinition<T> : FlowTransformerDefinitionBase<List<T>, List<T>>
    {
        internal FlowPartitionerDefinition(BlockInfo blockInfo, FlowTargetSettings settings) : base(blockInfo, settings, null, BlockType.Partitioner)
        {
        }

        public override void GenerateFlowPlanItem(IFlowSourceDefinition<List<T>> parent, IFlowPlan plan, FlowLinkDefinition<List<T>> link)
        {
            plan.AddPartitioner(parent.BlockInfo.Id, this, link);

            foreach(FlowLinkDefinition<List<T>> outboundLink in OutboundLinks)
                outboundLink.Target.GenerateFlowPlanItem(this, plan, outboundLink);
        }
    }
}