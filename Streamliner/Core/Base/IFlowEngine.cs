using System;
using Streamliner.Core.Utilities.Auditing;
using Streamliner.Definitions;

namespace Streamliner.Core.Base;

public interface IFlowEngine
{
    ILogger Logger { get; }
    IFlowAuditLogger AuditLogger { get; }
    IFlowPlan StartFlow(FlowDefinition definition);
    bool StopFlow(Guid planId);
}