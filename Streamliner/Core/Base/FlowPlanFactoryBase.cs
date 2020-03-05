using Streamliner.Definitions;

namespace Streamliner.Core.Base
{
    public abstract class FlowPlanFactoryBase
    {
        public abstract IFlowPlan GeneratePlan(FlowDefinition definition);
    }
}
