﻿using System;
using System.Collections.Generic;
using Streamliner.Core.Base;
using Streamliner.Core.Links.Local;
using Streamliner.Core.Links.Remote;
using Streamliner.Core.Links.Remote.MessageQueues;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions.Base
{
    public abstract class FlowTransformerDefinitionBase<TIn, TOut> : FlowDefinitionItemBase, IFlowSourceDefinition<TOut>, IFlowTargetDefinition<TIn>
    {
        public FlowTargetSettings Settings { get; }
        public ICollection<FlowLinkDefinition> InboundLinks { get; }
        public ICollection<FlowLinkDefinition<TOut>> OutboundLinks { get; }

        public abstract void GenerateFlowPlanItem(IFlowSourceDefinition<TIn> parent, IFlowPlan plan,
            FlowLinkDefinition<TIn> link);

        protected FlowTransformerDefinitionBase(BlockInfo blockInfo, FlowTargetSettings settings, Type actionType,
            BlockType blockType)
            : base(blockInfo, actionType, blockType)
        {
            Settings = settings;

            OutboundLinks = new List<FlowLinkDefinition<TOut>>();
            InboundLinks = new List<FlowLinkDefinition>();
        }

        public FlowLinkResult LinkTo(IFlowTargetDefinition<TOut> target, Func<TOut, bool> filterFunc = null)
        {
            FlowLinkDefinition<TOut> linkDefinition =
                new FlowLinkDefinition<TOut>(this, target, LocalLinkFactory.GetInstance(), filterFunc);

            return InternalLinkTo(target, linkDefinition);
        }

        public FlowLinkResult LinkTo(IFlowTargetDefinition<TOut> target, IMqFactory mqFactory, Func<TOut, bool> filterFunc)
        {
            FlowLinkDefinition<TOut> linkDefinition =
                new FlowLinkDefinition<TOut>(this, target, RemoteMqLinkFactory.GetInstance(mqFactory), filterFunc);

            return InternalLinkTo(target, linkDefinition);
        }

        private FlowLinkResult InternalLinkTo(IFlowTargetDefinition<TOut> target, FlowLinkDefinition<TOut> linkDefinition)
        {
            OutboundLinks.Add(linkDefinition);

            target.LinkFrom(linkDefinition);
            return new FlowLinkResult(linkDefinition);
        }

        public void LinkFrom(FlowLinkDefinition<TIn> link) => InboundLinks.Add(link);
    }
}