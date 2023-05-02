using System;
using System.Collections.Generic;
using Streamliner.Core.Base;
using Streamliner.Core.Links.Local;
using Streamliner.Core.Links.Remote;
using Streamliner.Core.Links.Remote.MessageQueues;
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
            
            return InternalLinkTo(linkDefinition);
        }

        public FlowLinkResult LinkTo(IFlowTargetDefinition<T> target, IMqFactory mqFactory, Func<T, bool> filterFunc)
        {
            FlowLinkDefinition<T> linkDefinition =
                new FlowLinkDefinition<T>(this, target, RemoteMqLinkFactory.GetInstance(mqFactory), filterFunc);

            return InternalLinkTo(linkDefinition);
        }
        
        private FlowLinkResult InternalLinkTo(FlowLinkDefinition<T> linkDefinition)
        {
            OutboundLinks.Add(linkDefinition);

            return new FlowLinkResult(linkDefinition);
        }
    }
}