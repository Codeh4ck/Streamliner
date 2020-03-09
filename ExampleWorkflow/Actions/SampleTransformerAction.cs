using System;
using System.Threading;
using ExampleWorkflow.Models;
using Streamliner.Actions;

namespace ExampleWorkflow.Actions
{
    public class SampleTransformerAction : TransformerBlockActionBase<ProducerModel, TransformerModel>
    {
        public override bool TryTransform(ProducerModel input, out TransformerModel model, CancellationToken token = default(CancellationToken))
        {
            model = new TransformerModel(TimeSpan.FromSeconds(5))
            {
                CycleId = input.CycleId,
                IterationNumber = input.IterationNumber,
                TimestampBeforeWait = DateTime.Now
            };

            return true;
        }
    }
}
