using System.Collections.Generic;
using Streamliner.Core.Base;
using Streamliner.Core.Links;
using Streamliner.Core.Links.Remote.MessageQueues;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions.Base
{
    public interface IFlowTargetDefinition<T> : IFlowDefinition
    {
        FlowTargetSettings Settings { get; }
        ICollection<FlowLinkDefinition> InboundLinks { get; }
        void LinkFrom(FlowLinkDefinition<T> link);
        void GenerateFlowPlanItem(IFlowSourceDefinition<T> parent, IFlowPlan plan, FlowLinkDefinition<T> link);
    }
}