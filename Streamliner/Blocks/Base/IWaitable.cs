using System;

namespace Streamliner.Blocks.Base
{
    public interface IWaitable
    {
        TimeSpan WaitFor { get; }
    }
}
