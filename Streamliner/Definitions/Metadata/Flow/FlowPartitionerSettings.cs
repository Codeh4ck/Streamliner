using System;
using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowPartitionerSettings : FlowTargetSettings
    {
        public ProducerType ProducerType { get; }
        public int PartitionSize { get; }
        public TimeSpan MaxPartitionTimeout { get; }

        public FlowPartitionerSettings(ProducerType producerType, int partitionSize, TimeSpan maxPartitionTimeout,
            int capacity, object context = null, uint maxDegreeOfParallelism = 1) : base(capacity, context,
            maxDegreeOfParallelism)
        {
            ProducerType = producerType;
            PartitionSize = partitionSize;
            MaxPartitionTimeout = maxPartitionTimeout;
        }
    }
}