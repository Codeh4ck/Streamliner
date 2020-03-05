using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent
{
    public class FluentProducerDefinition
    {
        private readonly ProducerType _producerType;
        private bool _enableAuditing = false;
        private object _withContext = null;
        private uint _parallelismInstances = 1;

        internal FluentProducerDefinition(ProducerType producerType)
        {
            _producerType = producerType;
        }

        public FluentProducerDefinition EnableAuditing()
        {
            _enableAuditing = true;
            return this;
        }

        public FluentProducerDefinition WithContext(object settings)
        {
            _withContext = settings;
            return this;
        }

        public FluentProducerDefinition WithParallelismInstances(uint instances)
        {
            _parallelismInstances = instances;
            return this;
        }

        public FluentProducerDefinitionThatProduces WithServiceInfo(Guid id, string name)
        {
            return new FluentProducerDefinitionThatProduces(new BlockInfo(id, name, BlockType.Producer),
                new FlowProducerSettings(_producerType, _withContext, _enableAuditing, _parallelismInstances));
        }
    }
}
