using Streamliner.Actions;
using Streamliner.Core;
using Streamliner.Core.Utilities;
using Streamliner.Fluent.Factories;
using Streamliner.TestFlow.Actions;
using Streamliner.TestFlow.Models;

namespace Streamliner.TestFlow
{
    public class Program
    {
        public static async Task Main()
        {

            var producer = ProducerDefinitionFactory
                .CreateDispatcher()
                .WithMaxDegreeOfParallelism(1)
                .WithBlockInfo(Guid.NewGuid(), "Producer")
                .ThatProduces<TestProducerModel>()
                .WithAction<TestProducerAction>();

            var transformer = TransformerDefinitionFactory
                .CreateDispatcher()
                .WithMaxDegreeOfParallelism(1)
                .WithBlockInfo(Guid.NewGuid(), "Transformer")
                .ThatTransforms<TestProducerModel, List<TestTransformerModel>>()
                .WithAction<TestTransformerAction>();

            var partitioner = PartitionerDefinitionFactory
                .CreateDispatcher()
                .WithPartitionSize(100)
                .WithMaxDegreeOfParallelism(1)
                .WithBlockInfo(Guid.NewGuid(), "Partitioner")
                .ThatPartitions<TestTransformerModel>();
            
            var consumer = ConsumerDefinitionFactory
                .CreateConsumer()
                .WithMaxDegreeOfParallelism(1)
                .WithBlockInfo(Guid.NewGuid(), "Consumer")
                .ThatConsumes<List<TestTransformerModel>>()
                .WithAction<TestConsumerAction>();

            producer.LinkTo(transformer);
            transformer.LinkTo(partitioner);
            partitioner.LinkTo(consumer);
            
            var flowDefinition = FlowDefinitionFactory.CreateStreamflow()
                .WithFlowInfo(Guid.NewGuid(), "Test Flow");
        
            flowDefinition.AddEntrypoint(producer);
            
            var registrar = new DependencyRegistrar();

            registrar.Register(() => new TestProducerAction());
            registrar.Register(() => new TestTransformerAction());
            registrar.Register(() => new TestConsumerAction());
            
            var blockActionFactory = new BlockActionIocFactory(registrar);
            var flowPlanFactory = new FlowPlanFactory(blockActionFactory);
            var flowEngine = new FlowEngine(flowPlanFactory);
            
            flowEngine.StartFlow(flowDefinition);

            Console.ReadKey();
        }
    }
}