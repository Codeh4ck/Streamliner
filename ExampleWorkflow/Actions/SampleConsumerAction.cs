using System;
using System.Threading;
using ExampleWorkflow.Models;
using Streamliner.Actions;

namespace ExampleWorkflow.Actions
{
    public class SampleConsumerAction : ConsumerBlockActionBase<TransformerModel>
    {
        public override void Consume(TransformerModel model, CancellationToken token = default(CancellationToken))
        {
            Console.WriteLine("---------------------------");

            Console.WriteLine($"Cycle ID: {model.CycleId}");
            Console.WriteLine($"Iteration number: {model.IterationNumber}");
            Console.WriteLine($"Timestamp before waiting: {model.TimestampBeforeWait:G}");
            Console.WriteLine($"Timestamp after waiting: {DateTime.Now:G}");

            Console.WriteLine("---------------------------");
        }
    }
}
