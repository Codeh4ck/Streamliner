﻿using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent
{
    public class FluentBatcherDefinitionThatBatches
    {
        private readonly BlockInfo _blockInfo;
        private readonly FlowBatcherSettings _batcherSettings;

        internal FluentBatcherDefinitionThatBatches(BlockInfo blockInfo, FlowBatcherSettings batcherSettings)
        {
            _blockInfo = blockInfo;
            _batcherSettings = batcherSettings;
        }

        public FlowBatcherDefinition<T> ThatBatches<T>()
        {
            return new FlowBatcherDefinition<T>(_blockInfo, _batcherSettings);
        }
    }
}
