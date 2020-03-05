using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Factories
{
    public static class FlowDefinitionFactory
    {
        public static FluentFlowDefinition CreateStreamflow() => new FluentFlowDefinition(FlowType.StreamFlow);
        public static FluentFlowDefinition CreateWorkflow() => new FluentFlowDefinition(FlowType.WorkFlow);
    }
}
