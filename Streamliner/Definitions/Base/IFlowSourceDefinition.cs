using System;
using System.Collections.Generic;

namespace Streamliner.Definitions.Base
{
    public interface IFlowSourceDefinition<T> : IFlowDefinition
    {
        ICollection<FlowLinkDefinition<T>> OutboundLinks { get; }
        FlowLinkResult LinkTo(IFlowTargetDefinition<T> target, Func<T, bool> filterFunc = null);
    }
}
