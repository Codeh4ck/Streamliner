﻿using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Definitions.Metadata.Flow
{
    public class FlowProducerSettings : FlowSettings
    {
        public ProducerType ProducerType { get; }
        public FlowProducerSettings(ProducerType producerType, object context = null, uint parallelismInstances = 1) 
            : base(context, parallelismInstances)
        {
            ProducerType = producerType;
        }
    }
}