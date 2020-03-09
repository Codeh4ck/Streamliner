using System;
using Streamliner.Core.Utilities.Auditing;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace ExampleFlow.Utilities
{
    public class AuditLogger : IFlowAuditLogger
    {
        public void BlockStarting(BlockHeader header)
        {
            Console.WriteLine("Block starting: " + header.BlockInfo.Name);
        }

        public void BlockStarted(BlockHeader header)
        {
            Console.WriteLine("Block started: " + header.BlockInfo.Name);
        }

        public void BlockProcessing(BlockHeader header, Guid cycleId)
        {
            Console.WriteLine("Block processing: " + header.BlockInfo.Name + " - Cycle ID: " + cycleId.ToString("N"));
        }

        public void BlockProcessed(BlockHeader header, Guid cycleId)
        {
            Console.WriteLine("Block processed: " + header.BlockInfo.Name + " - Cycle ID: " + cycleId.ToString("N"));
        }

        public void BlockStopping(BlockHeader header)
        {
            Console.WriteLine("Block stopping: " + header.BlockInfo.Name);
        }

        public void BlockStopped(BlockHeader header)
        {
            Console.WriteLine("Block stopped: " + header.BlockInfo.Name);
        }

        public void BlockPing(BlockHeader header)
        {
            Console.WriteLine("Block ping: " + header.BlockInfo.Name);
        }

        public void BlockException(BlockHeader header, Guid cycleId, Exception ex)
        {
            Console.WriteLine("Block exception: " + header.BlockInfo.Name + " - Cycle ID: " + cycleId.ToString("N") +
                              " - Exception: " + ex.Message);
        }

        public void FlowStarting(FlowServiceInfo serviceInfo)
        {
            Console.WriteLine("Flow starting: " + serviceInfo.Name);
        }

        public void FlowStarted(FlowServiceInfo serviceInfo)
        {
            Console.WriteLine("Block started: " + serviceInfo.Name);
        }

        public void FlowStopping(FlowServiceInfo serviceInfo)
        {
            Console.WriteLine("Block stopping: " + serviceInfo.Name);
        }

        public void FlowStopped(FlowServiceInfo serviceInfo)
        {
            Console.WriteLine("Block stopped: " + serviceInfo.Name);
        }
    }
}
