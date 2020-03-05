using System;
using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowBatcherSettings : FlowTargetSettings
    {
        public ProducerType ProducerType { get; }
        public int MaxBatchSize { get; }
        public TimeSpan MaxBatchTimeout { get; }

        public FlowBatcherSettings(ProducerType producerType, int maxBatchSize, TimeSpan maxBatchTimeout, int capacity, object context = null, bool enableAuditing = false, uint parallelismInstances = 1) : base(capacity, context, enableAuditing, parallelismInstances)
        {
            ProducerType = producerType;
            MaxBatchSize = maxBatchSize;
            MaxBatchTimeout = maxBatchTimeout;
        }
    }
}
