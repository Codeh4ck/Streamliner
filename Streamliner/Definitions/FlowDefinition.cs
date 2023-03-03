using System.Collections.Generic;
using Streamliner.Core.Utilities.Auditing;
using Streamliner.Definitions.Base;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions;

public sealed class FlowDefinition
{
    internal FlowDefinition(FlowServiceInfo serviceInfo, FlowType flowType)
    {
        Type = flowType;
        ServiceInfo = serviceInfo;

        Entrypoints = new();
    }

    public FlowType Type { get; }
    public uint Iterations { get; set; }
    public ILogger Logger { get; set; }
    public IFlowAuditLogger AuditLogger { get; set; }
    public FlowServiceInfo ServiceInfo { get; }
    public List<FlowProducerDefinitionBase> Entrypoints { get; }

    public void AddEntrypoint(FlowProducerDefinitionBase entrypoint)
    {
        Entrypoints.Add(entrypoint);
    }
}