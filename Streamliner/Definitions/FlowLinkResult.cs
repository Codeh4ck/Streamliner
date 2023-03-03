using Streamliner.Core.Links;

namespace Streamliner.Definitions;

public class FlowLinkResult
{
    private FlowLinkDefinition LinkDefinition { get; }

    internal FlowLinkResult(FlowLinkDefinition linkDefinition)
    {
        LinkDefinition = linkDefinition;
    }

    public void WithLinkFactory(IBlockLinkFactory linkFactory)
    {
        LinkDefinition.LinkFactory = linkFactory;
    }
}