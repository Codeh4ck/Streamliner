using System;
using ExampleWorkflow.Actions;
using ExampleWorkflow.Models;
using Streamliner.Actions;
using Streamliner.Core;
using Streamliner.Core.Utilities;
using Streamliner.Definitions;
using Streamliner.Fluent.Factories;

namespace ExampleWorkflow
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid flowId = Guid.NewGuid();
            string flowName = "Sample workflow";

            FlowDefinition definition = FlowDefinitionFactory
                .CreateWorkflow()
                .WithIterations(10)
                .WithFlowInfo(flowId, flowName);

            Guid producerId = Guid.NewGuid();
            string producerName = "Sample Workflow Producer";

            var producer = ProducerDefinitionFactory
                .CreateDispatcher()
                .WithParallelismInstances(1)
                .WithBlockInfo(producerId, producerName)
                .ThatProduces<ProducerModel>()
                .WithAction<SampleProducerAction>();

            Guid transformerId = Guid.NewGuid();
            string transformerName = "Sample Workflow Transformer";

            var transformer = TransformerDefinitionFactory
                .CreateDispatcher()
                .WithParallelismInstances(1)
                .WithCapacity(1)
                .WithBlockInfo(transformerId, transformerName)
                .ThatTransforms<ProducerModel, TransformerModel>()
                .WithAction<SampleTransformerAction>();

            Guid waiterId = Guid.NewGuid();
            string waiterName = "Sample Workflow Waiter";

            var waiter = WaiterDefinitionFactory
                .CreateDispatcher()
                .WithParallelismInstances(1)
                .WithCapacity(1)
                .WithBlockInfo(waiterId, waiterName)
                .ThatWaits<TransformerModel>();

            Guid consumerId = Guid.NewGuid();
            string consumerName = "Sample Workflow Consumer";

            var consumer = ConsumerDefinitionFactory
                .CreateConsumer()
                .WithParallelismInstances(1)
                .WithCapacity(1)
                .WithBlockInfo(consumerId, consumerName)
                .ThatConsumes<TransformerModel>()
                .WithAction<SampleConsumerAction>();

            producer.LinkTo(transformer);
            transformer.LinkTo(waiter);
            waiter.LinkTo(consumer);

            definition.AddEntrypoint(producer);

            DependencyRegistrar registrar = new DependencyRegistrar();

            registrar.Register<SampleProducerAction>(() => new SampleProducerAction());
            registrar.Register<SampleTransformerAction>(() => new SampleTransformerAction());
            registrar.Register<SampleConsumerAction>(() => new SampleConsumerAction());

            IBlockActionFactory actionFactory = new BlockActionIocFactory(registrar);

            FlowPlanFactory planFactory = new FlowPlanFactory(actionFactory);
            
            FlowEngine engine = new FlowEngine(planFactory);
            engine.StartFlow(definition);

            Console.ReadKey();
        }
    }
}
