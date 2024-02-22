using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Producer
{
    public sealed class FluentProducerDefinition
    {
        private readonly ProducerType _producerType;
        private object _withContext = null;
        private uint _maxDegreeOfParallelism = 1;

        internal FluentProducerDefinition(ProducerType producerType) => _producerType = producerType;

        public FluentProducerDefinition WithContext(object settings)
        {
            _withContext = settings;
            return this;
        }

        public FluentProducerDefinition WithMaxDegreeOfParallelism(uint degree)
        {
            _maxDegreeOfParallelism = degree;
            return this;
        }

        public FluentProducerDefinitionThatProduces WithBlockInfo(Guid id, string name) =>
            new FluentProducerDefinitionThatProduces(new BlockInfo(id, name, BlockType.Producer),
                new FlowProducerSettings(_producerType, _withContext, _maxDegreeOfParallelism));
    }
}