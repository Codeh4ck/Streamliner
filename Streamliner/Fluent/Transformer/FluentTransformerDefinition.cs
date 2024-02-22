using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Transformer
{
    public sealed class FluentTransformerDefinition
    {
        private readonly ProducerType _producerType;
        private object _withContext = null;
        private int _capacity = 1;
        private uint _maxDegreeOfParallelism = 1;

        internal FluentTransformerDefinition(ProducerType producerType) => _producerType = producerType;

        public FluentTransformerDefinition WithContext(object settings)
        {
            _withContext = settings;
            return this;
        }

        public FluentTransformerDefinition WithMaxDegreeOfParallelism(uint degree)
        {
            _maxDegreeOfParallelism = degree;
            return this;
        }

        public FluentTransformerDefinition WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public FluentTransformerDefinitionThatTransforms WithBlockInfo(Guid id, string name) =>
            new FluentTransformerDefinitionThatTransforms(new BlockInfo(id, name, BlockType.Producer),
                new FlowTransformerSettings(_producerType, _capacity, _withContext, _maxDegreeOfParallelism));
    }
}