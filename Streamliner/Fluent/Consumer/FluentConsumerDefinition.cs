using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Consumer
{
    public sealed class FluentConsumerDefinition
    {
        private object _withContext = null;
        private int _capacity = 1;
        private uint _maxDegreeOfParallelism = 1;

        internal FluentConsumerDefinition() { }

        public FluentConsumerDefinition WithContext(object settings)
        {
            _withContext = settings;
            return this;
        }

        public FluentConsumerDefinition WithMaxDegreeOfParallelism(uint degree)
        {
            _maxDegreeOfParallelism = degree;
            return this;
        }

        public FluentConsumerDefinition WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public FluentConsumerDefinitionThatConsumes WithBlockInfo(Guid id, string name) =>
            new FluentConsumerDefinitionThatConsumes(new BlockInfo(id, name, BlockType.Producer),
                new FlowConsumerSettings(_capacity, _withContext, _maxDegreeOfParallelism));
    }
}