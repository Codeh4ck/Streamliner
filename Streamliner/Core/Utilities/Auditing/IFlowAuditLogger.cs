using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Core.Utilities.Auditing;

public interface IFlowAuditLogger
{
    void BlockStarting(BlockHeader header);
    void BlockStarted(BlockHeader header);
    void BlockProcessing(BlockHeader header, Guid cycleId);
    void BlockProcessed(BlockHeader header, Guid cycleId);
    void BlockStopping(BlockHeader header);
    void BlockStopped(BlockHeader header);
    void BlockPing(BlockHeader header);
    void BlockException(BlockHeader header, Guid cycleId, Exception ex);
    void FlowStarting(FlowServiceInfo serviceInfo);
    void FlowStarted(FlowServiceInfo serviceInfo);
    void FlowStopping(FlowServiceInfo serviceInfo);
    void FlowStopped(FlowServiceInfo serviceInfo);
}