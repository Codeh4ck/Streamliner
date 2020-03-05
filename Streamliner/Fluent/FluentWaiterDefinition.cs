using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent
{
    public class FluentWaiterDefinition
    {
        private readonly ProducerType _producerType;
        private bool _enableAuditing = false;
        private object _withContext = null;
        private int _capacity = 1;
        private uint _parallelismInstances = 1;

        internal FluentWaiterDefinition(ProducerType producerType)
        {
            _producerType = producerType;
        }

        public FluentWaiterDefinition EnableAuditing()
        {
            _enableAuditing = true;
            return this;
        }

        public FluentWaiterDefinition WithContext(object settings)
        {
            _withContext = settings;
            return this;
        }

        public FluentWaiterDefinition WithParallelismInstances(uint instances)
        {
            _parallelismInstances = instances;
            return this;
        }

        public FluentWaiterDefinition WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public FluentWaiterDefinitionThatWaits WithServiceInfo(Guid id, string name)
        {
            return new FluentWaiterDefinitionThatWaits(new BlockInfo(id, name, BlockType.Producer),
                new FlowWaiterSettings(_producerType, _capacity, _withContext, _enableAuditing, _parallelismInstances));
        }
    }
}
