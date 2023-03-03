using System;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Flow;

public class FluentFlowDefinition
{
    private readonly FlowType _flowType;
    private uint _iterations = 1;

    internal FluentFlowDefinition(FlowType flowType) => _flowType = flowType;

    public FluentFlowDefinition WithIterations(uint iterations)
    {
        _iterations = iterations;
        return this;
    }

    public FlowDefinition WithFlowInfo(Guid id, string name) =>
        new(new(id, name), _flowType)
        {
            Iterations = _iterations,
        };
}