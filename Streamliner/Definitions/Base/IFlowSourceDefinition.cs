using System;
using System.Collections.Generic;
using Streamliner.Core.Links;
using Streamliner.Core.Links.Remote.MessageQueues;

namespace Streamliner.Definitions.Base
{
    public interface IFlowSourceDefinition<T> : IFlowDefinition
    {
        ICollection<FlowLinkDefinition<T>> OutboundLinks { get; }
        FlowLinkResult LinkTo(IFlowTargetDefinition<T> target, Func<T, bool> filterFunc = null);
        FlowLinkResult LinkTo(IFlowTargetDefinition<T> target, IMqFactory mqFactory, Func<T, bool> filterFunc);
    }
}