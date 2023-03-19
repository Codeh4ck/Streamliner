using Streamliner.Blocks.Base;
using Streamliner.Core.Base;
using Streamliner.Definitions.Base;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions
{
    public sealed class FlowWaiterDefinition<T> : FlowTransformerDefinitionBase<T, T> where T: IWaitable
    {
        internal FlowWaiterDefinition(BlockInfo blockInfo, FlowWaiterSettings settings) : base(blockInfo, settings, null, BlockType.Waiter)
        {
        }

        public override void GenerateFlowPlanItem(IFlowSourceDefinition<T> parent, IFlowPlan plan, FlowLinkDefinition<T> link)
        {
            plan.AddWaiter(parent.BlockInfo.Id, this, link);
         
            foreach (FlowLinkDefinition<T> outboundLink in OutboundLinks)
                outboundLink.Target.GenerateFlowPlanItem(this, plan, outboundLink);
        }
    }
}