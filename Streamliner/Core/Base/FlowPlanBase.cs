using System;
using Streamliner.Actions;
using Streamliner.Blocks.Base;
using Streamliner.Core.Utilities;
using Streamliner.Core.Utilities.Auditing;
using Streamliner.Definitions;

namespace Streamliner.Core.Base
{
    internal abstract class FlowPlanBase : Runnable, IFlowPlan
    {
        public ILogger Logger { get; }
        public IFlowAuditLogger AuditLogger { get; }
        public FlowDefinition Definition { get; }

        protected FlowPlanBase(FlowDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            
            Definition = definition;
            Logger = definition.Logger;
            AuditLogger = definition.AuditLogger;
        }

        public abstract void AddProducer<T>(FlowProducerDefinition<T> definition);

        public abstract void AddConsumer<T>(Guid parentId, FlowConsumerDefinition<T> definition, FlowLinkDefinition<T> link);

        public abstract void AddTransformer<TIn, TOut>(Guid parentId,
            FlowTransformerDefinition<TIn, TOut> definition, FlowLinkDefinition<TIn> link);

        public abstract void AddWaiter<T>(Guid parentId, FlowWaiterDefinition<T> definition, FlowLinkDefinition<T> link)
            where T : IWaitable;

        public abstract void AddBatcher<T>(Guid parentId, FlowBatcherDefinition<T> definition, FlowLinkDefinition<T> link);

        public abstract void Wait();
        public abstract void Trigger<T>(TriggerContext<T> triggerContext);
    }
}
