﻿using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent
{
    public class FluentConsumerDefinition
    {
        private bool _enableAuditing = false;
        private object _withContext = null;
        private int _capacity = 1;
        private uint _parallelismInstances = 1;

        internal FluentConsumerDefinition() { }

        public FluentConsumerDefinition EnableAuditing()
        {
            _enableAuditing = true;
            return this;
        }

        public FluentConsumerDefinition WithContext(object settings)
        {
            _withContext = settings;
            return this;
        }

        public FluentConsumerDefinition WithParallelismInstances(uint instances)
        {
            _parallelismInstances = instances;
            return this;
        }

        public FluentConsumerDefinition WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public FluentConsumerDefinitionThatConsumes WithServiceInfo(Guid id, string name)
        {
            return new FluentConsumerDefinitionThatConsumes(new BlockInfo(id, name, BlockType.Producer),
                new FlowConsumerSettings(_capacity, _withContext, _enableAuditing, _parallelismInstances));
        }
    }
}
