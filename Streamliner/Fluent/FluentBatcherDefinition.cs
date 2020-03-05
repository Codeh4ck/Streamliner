﻿using System;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent
{
    public class FluentBatcherDefinition
    {
        private readonly ProducerType _producerType;

        private int _maxBatchSize;
        private TimeSpan _maxBatchTimeout;
        private bool _enableAuditing = false;
        private object _withContext = null;
        private int _capacity = 1;
        private uint _parallelismInstances = 1;

        internal FluentBatcherDefinition(ProducerType producerType)
        {
            _producerType = producerType;
        }

        public FluentBatcherDefinition EnableAuditing()
        {
            _enableAuditing = true;
            return this;
        }

        public FluentBatcherDefinition WithContext(object settings)
        {
            _withContext = settings;
            return this;
        }

        public FluentBatcherDefinition WithParallelismInstances(uint instances)
        {
            _parallelismInstances = instances;
            return this;
        }

        public FluentBatcherDefinition WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public FluentBatcherDefinition WithMaxBatchSize(int maxBatchSize)
        {
            _maxBatchSize = maxBatchSize;
            return this;
        }

        public FluentBatcherDefinition WithMaxBatchTimeout(TimeSpan maxBatchTimeout)
        {
            _maxBatchTimeout = maxBatchTimeout;
            return this;
        }

        public FluentBatcherDefinitionThatBatches WithServiceInfo(Guid id, string name)
        {
            return new FluentBatcherDefinitionThatBatches(new BlockInfo(id, name, BlockType.Producer),
                new FlowBatcherSettings(_producerType, _maxBatchSize, _maxBatchTimeout, _capacity, _withContext,
                    _enableAuditing, _parallelismInstances));
        }
    }
}
