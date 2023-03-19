using System;
using Streamliner.Core.Links;
using Streamliner.Definitions.Base;

namespace Streamliner.Definitions
{
    public class FlowLinkDefinition
    {
        public IBlockLinkFactory LinkFactory { get; set; }

        internal FlowLinkDefinition(IBlockLinkFactory linkFactory)
        {
            LinkFactory = linkFactory ?? throw new ArgumentNullException(nameof(linkFactory));
        }
    }

    public sealed class FlowLinkDefinition<T> : FlowLinkDefinition
    {
        public IFlowSourceDefinition<T> Source { get; }
        public IFlowTargetDefinition<T> Target { get; }
        public Func<T, bool> FuncFilter { get; }

        internal FlowLinkDefinition(IFlowSourceDefinition<T> source, IFlowTargetDefinition<T> target, IBlockLinkFactory linkFactory) : base(linkFactory)
        {
            Source = source;
            Target = target;
        }

        internal FlowLinkDefinition(IFlowSourceDefinition<T> source, IFlowTargetDefinition<T> target,
            IBlockLinkFactory linkFactory, Func<T, bool> funcFilter) : this(source, target, linkFactory)
        {
            FuncFilter = funcFilter;
        }
    }
}