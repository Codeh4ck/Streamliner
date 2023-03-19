using System;
using System.Collections.Generic;
using Streamliner.Core.Base;
using Streamliner.Core.Links.Local;
using Streamliner.Definitions.Base;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions
{
    public class FlowProducerDefinition<T> : FlowProducerDefinitionBase, IFlowSourceDefinition<T>
    {
        public ICollection<FlowLinkDefinition<T>> OutboundLinks { get; }

        internal FlowProducerDefinition(BlockInfo blockInfo, FlowProducerSettings settings, Type actionType) : base(blockInfo, settings, actionType)
        {
            OutboundLinks = new List<FlowLinkDefinition<T>>();
        }

        public override void GeneratePlanItem(IFlowPlan plan)
        {
            plan.AddProducer(this);

            foreach(FlowLinkDefinition<T> link in OutboundLinks)
                link.Target.GenerateFlowPlanItem(this, plan, link);
        }

        public FlowLinkResult LinkTo(IFlowTargetDefinition<T> target, Func<T, bool> filterFunc = null)
        {
            FlowLinkDefinition<T> linkDefinition =
                new FlowLinkDefinition<T>(this, target, LocalLinkFactory.GetInstance(), filterFunc);
            OutboundLinks.Add(linkDefinition);

            return new FlowLinkResult(linkDefinition);
        }
    }
}