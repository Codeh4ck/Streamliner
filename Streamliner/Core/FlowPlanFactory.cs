﻿using System;
using Streamliner.Actions;
using Streamliner.Core.Base;
using Streamliner.Definitions;
using Streamliner.Definitions.Base;

namespace Streamliner.Core
{
    public class FlowPlanFactory : FlowPlanFactoryBase
    {
        private readonly IBlockActionFactory _actionFactory;

        public FlowPlanFactory(IBlockActionFactory actionFactory) => 
            _actionFactory = actionFactory ?? throw new ArgumentNullException(nameof(actionFactory));

        public override IFlowPlan GeneratePlan(FlowDefinition definition)
        {
            FlowPlanInternal internalPlan = new FlowPlanInternal(_actionFactory, definition);

            foreach(FlowProducerDefinitionBase entrypoint in definition.Entrypoints)
                entrypoint.GeneratePlanItem(internalPlan);

            return internalPlan;
        }
    }
}