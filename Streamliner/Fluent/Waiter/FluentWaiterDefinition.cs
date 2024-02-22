using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Waiter
{
    public sealed class FluentWaiterDefinition
    {
        private readonly ProducerType _producerType;
        private object _withContext = null;
        private int _capacity = 1;
        private uint _maxDegreeOfParallelism = 1;

        internal FluentWaiterDefinition(ProducerType producerType) => _producerType = producerType;

        public FluentWaiterDefinition WithContext(object settings)
        {
            _withContext = settings;
            return this;
        }

        public FluentWaiterDefinition WithMaxDegreeOfParallelism(uint degree)
        {
            _maxDegreeOfParallelism = degree;
            return this;
        }

        public FluentWaiterDefinition WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public FluentWaiterDefinitionThatWaits WithBlockInfo(Guid id, string name) =>
            new FluentWaiterDefinitionThatWaits(new BlockInfo(id, name, BlockType.Producer),
                new FlowWaiterSettings(_producerType, _capacity, _withContext, _maxDegreeOfParallelism));
    }
}