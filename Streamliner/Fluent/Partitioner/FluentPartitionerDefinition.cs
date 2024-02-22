using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Partitioner
{
    public sealed class FluentPartitionerDefinition
    {
        private readonly ProducerType _producerType;

        private int _partitionSize;
        private TimeSpan _maxPartitionTimeout;
        private object _withContext = null;
        private int _capacity = 1;
        private uint _maxDegreeOfParallelism = 1;

        internal FluentPartitionerDefinition(ProducerType producerType) => _producerType = producerType;

        public FluentPartitionerDefinition WithContext(object settings)
        {
            _withContext = settings;
            return this;
        }

        public FluentPartitionerDefinition WithMaxDegreeOfParallelism(uint degree)
        {
            _maxDegreeOfParallelism = degree;
            return this;
        }

        public FluentPartitionerDefinition WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public FluentPartitionerDefinition WithPartitionSize(int partitionSize)
        {
            _partitionSize = partitionSize;
            return this;
        }

        public FluentPartitionerDefinition WithMaxBatchTimeout(TimeSpan maxBatchTimeout)
        {
            _maxPartitionTimeout = maxBatchTimeout;
            return this;
        }

        public FluentPartitionerDefinitionThatPartitions WithBlockInfo(Guid id, string name) =>
            new FluentPartitionerDefinitionThatPartitions(new BlockInfo(id, name, BlockType.Producer),
                new FlowPartitionerSettings(_producerType, _partitionSize, _maxPartitionTimeout, _capacity, _withContext, _maxDegreeOfParallelism));
    }
}