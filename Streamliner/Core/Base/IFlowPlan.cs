using System;
using System.Collections.Generic;
using Streamliner.Blocks.Base;
using Streamliner.Core.Utilities;
using Streamliner.Core.Utilities.Auditing;
using Streamliner.Definitions;

namespace Streamliner.Core.Base
{
    public interface IFlowPlan: IRunnable
    {
        ILogger Logger { get; }
        IFlowAuditLogger AuditLogger { get; }
        FlowDefinition Definition { get; }
        void AddProducer<T>(FlowProducerDefinition<T> producer);
        void AddConsumer<T>(Guid parentId, FlowConsumerDefinition<T> consumer, FlowLinkDefinition<T> link);
        void AddTransformer<TIn, TOut>(Guid parentId, FlowTransformerDefinition<TIn, TOut> transformer, FlowLinkDefinition<TIn> link);
        void AddWaiter<T>(Guid parentId, FlowWaiterDefinition<T> waiter, FlowLinkDefinition<T> link) where T : IWaitable;
        void AddBatcher<T>(Guid parentId, FlowBatcherDefinition<T> batcher, FlowLinkDefinition<T> link);
        void AddPartitioner<T>(Guid parentId, FlowPartitionerDefinition<T> partitioner, FlowLinkDefinition<List<T>> link);
    }
}