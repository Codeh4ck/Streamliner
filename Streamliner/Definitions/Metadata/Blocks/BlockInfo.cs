using System;
using Streamliner.Core.Base;

namespace Streamliner.Definitions.Metadata.Blocks
{
    public class BlockInfo : ServiceInfo
    {
        public BlockInfo(Guid id, string name, BlockType blockType) : base(id, name)
        {
            BlockType = blockType;
        }

        public BlockInfo(Guid id, string name, string description, BlockType blockType) : base(id, name, description)
        {
            BlockType = blockType;
        }

        public BlockType BlockType { get; set; }
    }
}