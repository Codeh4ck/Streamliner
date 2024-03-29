﻿using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Fluent.Transformer
{
    public class FluentTransformerDefinitionThatTransforms
    {
        private readonly BlockInfo _blockInfo;
        private readonly FlowTransformerSettings _transformerSettings;

        internal FluentTransformerDefinitionThatTransforms(BlockInfo blockInfo, FlowTransformerSettings transformerSettings)
        {
            _blockInfo = blockInfo;
            _transformerSettings = transformerSettings;
        }

        public FluentTransformerDefinitionWithAction<TIn, TOut> ThatTransforms<TIn, TOut>() =>
            new FluentTransformerDefinitionWithAction<TIn, TOut>(_blockInfo, _transformerSettings);
    }
}