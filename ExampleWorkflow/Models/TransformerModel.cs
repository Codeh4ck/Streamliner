using System;
using Streamliner.Blocks.Base;

namespace ExampleWorkflow.Models
{
    public class TransformerModel : IWaitable
    {
        public TransformerModel(TimeSpan waitFor)
        {
            WaitFor = waitFor;
        }

        public Guid CycleId { get; set; }
        public int IterationNumber { get; set; }
        public DateTime TimestampBeforeWait { get; set; }
        public DateTime TimestampAfterWait { get; set; }

        // Determines how long the waiter is going to wait when receiving this model
        public TimeSpan WaitFor { get; }
    }
}
