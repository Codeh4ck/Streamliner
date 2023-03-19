using System;
using Streamliner.Core.Base;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Definitions.Base
{
    public abstract class FlowProducerDefinitionBase : FlowDefinitionItemBase
    {
        public FlowProducerSettings Settings { get; }

        protected FlowProducerDefinitionBase(BlockInfo blockInfo, FlowProducerSettings settings, Type actionType) :
            base(blockInfo, actionType, BlockType.Producer) => Settings = settings;

        public abstract void GeneratePlanItem(IFlowPlan plan);
    }
}