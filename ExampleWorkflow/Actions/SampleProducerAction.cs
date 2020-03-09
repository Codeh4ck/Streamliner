using System;
using System.Threading;
using ExampleWorkflow.Models;
using Streamliner.Actions;

namespace ExampleWorkflow.Actions
{
    public class SampleProducerAction : ProducerBlockActionBase<ProducerModel>
    {
        private static int _iteration = 0;

        public override bool TryProduce(out ProducerModel model, CancellationToken token = default(CancellationToken))
        {
            model = new ProducerModel()
            {
                CycleId = Guid.NewGuid(),
                IterationNumber = _iteration
            };

            _iteration++;

            return true;
        }
    }
}
