using Streamliner.Actions;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent
{
    public class FluentTransformerDefinitionWithAction<TIn, TOut>
    {
        private readonly BlockInfo _blockInfo;
        private readonly FlowTransformerSettings _transformerSettings;

        internal FluentTransformerDefinitionWithAction(BlockInfo blockInfo, FlowTransformerSettings transformerSettings)
        {
            _blockInfo = blockInfo;
            _transformerSettings = transformerSettings;
        }

        public FlowTransformerDefinition<TIn, TOut> WithAction<TAction>() where TAction : TransformerBlockActionBase<TIn, TOut>
        {
            return new FlowTransformerDefinition<TIn, TOut>(_blockInfo, _transformerSettings, typeof(TAction));
        }
    }
}
