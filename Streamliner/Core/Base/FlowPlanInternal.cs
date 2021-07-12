using System;
using System.Collections.Generic;
using System.Linq;
using Streamliner.Actions;
using Streamliner.Blocks;
using Streamliner.Blocks.Base;
using Streamliner.Core.Links;
using Streamliner.Core.Routing;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Core.Base
{
    internal class FlowPlanInternal : FlowPlanBase
    {
        private readonly IBlockActionFactory _actionFactory;
        private readonly FlowBlockContainer _blockContainer;

        private readonly FlowType _flowType;
        private readonly uint _iterations;

        public FlowPlanInternal(IBlockActionFactory actionFactory, FlowDefinition definition) : base(definition)
        {
            if (actionFactory == null) throw new ArgumentNullException(nameof(actionFactory));

            _actionFactory = actionFactory;
            _blockContainer = new FlowBlockContainer();

            _flowType = definition.Type;
            _iterations = definition.Iterations;
        }

        public override void AddProducer<T>(FlowProducerDefinition<T> definition)
        {
            ProducerBlockActionBase<T> action = _actionFactory.CreateProducerAction<T>(definition.ActionType);
            AddProducer(definition, action);
        }

        protected void AddProducer<T>(FlowProducerDefinition<T> definition, ProducerBlockActionBase<T> action)
        {
            BlockHeader header = new BlockHeader(definition.BlockInfo, Definition.ServiceInfo);
            action.Header = header;
            action.Context = definition.Settings.Context;

            definition.Settings.Type = _flowType;
            definition.Settings.Iterations = _iterations;

            if (action.Logger == null)
                action.Logger = Logger;

            LinkRouterBase<T> router = GetLinkFromProducerType<T>(definition.Settings.ProducerType);
            ProducerBlock<T> block = new ProducerBlock<T>(header, router, action, definition);

            AssignLoggers(block);
            _blockContainer.AddProducer(block);
        }

        public override void AddConsumer<T>(Guid parentId, FlowConsumerDefinition<T> definition, FlowLinkDefinition<T> link)
        {
            if (!_blockContainer.TryGetSourceBlock(parentId, out SourceBlockBase<T> parentBlock))
            {
                throw new Exception($"Cannot link block {definition.BlockInfo.Name} with id {definition.BlockInfo.Id} to parent block. " +
                                    "Parent block not found.");
            }

            if (!_blockContainer.TryGetConsumer(definition.BlockInfo.Id, out ConsumerBlock<T> consumer))
            {
                BlockHeader header = new BlockHeader(definition.BlockInfo, Definition.ServiceInfo);
                ConsumerBlockActionBase<T> action = _actionFactory.CreateConsumerAction<T>(definition.ActionType);

                action.Header = header;
                action.Context = definition.Settings.Context;

                definition.Settings.Type = _flowType;
                definition.Settings.Iterations = _iterations;

                action.Logger ??= Logger;

                IBlockLinkReceiver<T> receiver = link.LinkFactory.CreateReceiver(link);
                consumer = new ConsumerBlock<T>(header, receiver, action, definition);

                AssignLoggers(consumer);

                _blockContainer.AddBlock(consumer);
            }

            Link(parentBlock, consumer, link);
        }

        public override void AddTransformer<TIn, TOut>(Guid parentId, FlowTransformerDefinition<TIn, TOut> definition, FlowLinkDefinition<TIn> link)
        {
            FlowTransformerSettings settings = (FlowTransformerSettings) definition.Settings;
            LinkRouterBase<TOut> router = GetLinkFromProducerType<TOut>(settings.ProducerType);

            if (!_blockContainer.TryGetTransformer(definition.BlockInfo.Id, out TransformerBlock<TIn, TOut> transformer))
            {
                BlockHeader header = new BlockHeader(definition.BlockInfo, Definition.ServiceInfo);
                TransformerBlockActionBase<TIn, TOut> action = _actionFactory.CreateTransformerAction<TIn, TOut>(definition.ActionType);

                action.Header = header;
                action.Context = definition.Settings.Context;
               
                definition.Settings.Type = _flowType;
                definition.Settings.Iterations = _iterations;

                action.Logger ??= Logger;

                IBlockLinkReceiver<TIn> receiver = link.LinkFactory.CreateReceiver(link);
                transformer = new TransformerBlock<TIn, TOut>(header, receiver, router, action, definition);
                
                AssignLoggers(transformer);
                _blockContainer.AddBlock(transformer);
            }

            if (_blockContainer.TryGetSourceBlock(parentId, out SourceBlockBase<TIn> parentBlock))
            {
                Link(parentBlock, transformer, link);
                return;
            }

            throw new Exception($"Cannot link block {definition.BlockInfo.Name} with id {definition.BlockInfo.Id} to parent block. " +
                                "Either the parent block or the child block was not found.");
        }

        public override void AddWaiter<T>(Guid parentId, FlowWaiterDefinition<T> definition, FlowLinkDefinition<T> link)
        {
            if (!_blockContainer.TryGetSourceBlock(parentId, out SourceBlockBase<T> parentBlock))
            {
                throw new Exception($"Cannot link block {definition.BlockInfo.Name} with id {definition.BlockInfo.Id} to parent block. " +
                                    "Parent block not found.");
            }

            if (!_blockContainer.TryGetBlock(definition.BlockInfo.Id, out WaiterBlock<T> waiter))
            {
                BlockHeader header = new BlockHeader(definition.BlockInfo, Definition.ServiceInfo);
                FlowWaiterSettings settings = (FlowWaiterSettings) definition.Settings;

                definition.Settings.Type = _flowType;
                definition.Settings.Iterations = _iterations;

                LinkRouterBase<T> router = GetLinkFromProducerType<T>(settings.ProducerType);
                IBlockLinkReceiver<T> receiver = link.LinkFactory.CreateReceiver(link);

                waiter = new WaiterBlock<T>(header, receiver, router, definition);
                AssignLoggers(waiter);
                _blockContainer.AddBlock(waiter);
            }

            Link(parentBlock, waiter, link);
        }

        public override void AddBatcher<T>(Guid parentId, FlowBatcherDefinition<T> definition, FlowLinkDefinition<T> link)
        {
            if (!_blockContainer.TryGetSourceBlock(parentId, out SourceBlockBase<T> parentBlock))
            {
                throw new Exception($"Cannot link block {definition.BlockInfo.Name} with id {definition.BlockInfo.Id} to parent block. " +
                                    "Parent block not found.");
            }

            if (!_blockContainer.TryGetBlock(definition.BlockInfo.Id, out BatcherBlock<T> batcher))
            {
                BlockHeader header = new BlockHeader(definition.BlockInfo, Definition.ServiceInfo);
                FlowBatcherSettings settings = (FlowBatcherSettings) definition.Settings;

                definition.Settings.Type = _flowType;
                definition.Settings.Iterations = _iterations;

                LinkRouterBase<List<T>> router = GetLinkFromProducerType<List<T>>(settings.ProducerType);
                IBlockLinkReceiver<T> receiver = link.LinkFactory.CreateReceiver(link);

                batcher = new BatcherBlock<T>(header, receiver, router, definition);

                AssignLoggers(batcher);
                _blockContainer.AddBlock(batcher);
            }

            Link(parentBlock, batcher, link);
        }

        public override void Wait()
        {
            foreach (BlockBase blockBase in _blockContainer.Entrypoints)
                blockBase.Wait();
        }

        public override void Trigger<T>(TriggerContext<T> triggerContext)
        {
            ProducerBlock<T> producer = null;
            
            if (triggerContext.Id != null)
            {
                Guid value = triggerContext.Id.Value;

                if(!_blockContainer.TryGetProducer(value, out producer))
                    throw new Exception($"Cannot find producer with id {value}. Unable to trigger flow plan.");
            }
            else
                producer = (ProducerBlock<T>)_blockContainer.Entrypoints.First();
            
            producer.Trigger(triggerContext.Item);
        }

        protected override void OnStart(object context = null)
        {
            AuditLogger?.FlowStarting(Definition.ServiceInfo);

            foreach (BlockBase block in _blockContainer.Entrypoints)
                block.Start();

            AuditLogger?.FlowStarted(Definition.ServiceInfo);
        }

        protected override void OnStop()
        {
            AuditLogger?.FlowStopping(Definition.ServiceInfo);

            foreach (BlockBase block in _blockContainer.Blocks)
                block.Stop();

            AuditLogger?.FlowStopped(Definition.ServiceInfo);
        }

        protected virtual void Link<T>(ISourceBlock<T> from, ITargetBlock<T> to, FlowLinkDefinition<T> link)
        {
            IBlockLink<T> blockLink = link.LinkFactory.CreateLink(from, to, link);
            from.AddLink(blockLink);
        }

        private LinkRouterBase<T> GetLinkFromProducerType<T>(ProducerType type)
        {
            switch (type)
            {
                case ProducerType.Broadcaster:
                    return new LinkBroadcasterRouter<T>();
                case ProducerType.Dispatcher:
                    return new LinkDispatcherRouter<T>();
                default:
                    throw new Exception("Unknown producer type specified. Cannot create router.");
            }
        }

        private void AssignLoggers(BlockBase block)
        {
            block.Logger = Logger;
            block.AuditLogger = AuditLogger;
        }
    }
}
