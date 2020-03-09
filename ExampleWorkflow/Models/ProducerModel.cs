using System;

namespace ExampleWorkflow.Models
{
    public class ProducerModel
    {
        public Guid CycleId { get; set; }
        public int IterationNumber { get; set; }
    }
}
