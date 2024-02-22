using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Partitioner
{
    public sealed class FluentPartitionerDefinitionThatPartitions
    {
        private readonly BlockInfo _blockInfo;
        private readonly FlowPartitionerSettings _partitionerSettings;

        internal FluentPartitionerDefinitionThatPartitions(BlockInfo blockInfo, FlowPartitionerSettings partitionerSettings)
        {
            _blockInfo = blockInfo;
            _partitionerSettings = partitionerSettings;
        }

        public FlowPartitionerDefinition<T> ThatPartitions<T>() => new FlowPartitionerDefinition<T>(_blockInfo, _partitionerSettings);
    }
}