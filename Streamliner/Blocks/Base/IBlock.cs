﻿using Streamliner.Core.Utilities;
using Streamliner.Definitions.Metadata.Blocks;

namespace Streamliner.Blocks.Base
{
    public interface IBlock : IRunnable
    {
        BlockHeader Header { get; }
        void Wait();
    }
}