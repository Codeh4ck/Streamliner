using System;
using System.Collections.Generic;
using ExampleFlow.Actions;
using ExampleFlow.Models;
using ExampleFlow.Utilities;
using Streamliner.Actions;
using Streamliner.Core;
using Streamliner.Core.Base;
using Streamliner.Core.Utilities;
using Streamliner.Definitions;
using Streamliner.Fluent.Factories;

namespace ExampleFlow
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Constructing a FlowDefinition
            Guid flowId = Guid.NewGuid();
            string flowName = "Sample User Registration Flow";

            FlowDefinition definition = FlowDefinitionFactory
                .CreateStreamflow()
                .WithFlowInfo(flowId, flowName);
            #endregion

            #region Constructing the blocks
            Guid producerId = Guid.NewGuid();
            string producerName = "Registration request producer";

            var producer = ProducerDefinitionFactory
                .CreateDispatcher()
                .WithParallelismInstances(1)
                .WithBlockInfo(producerId, producerName)
                .ThatProduces<UserRegistrationRequest>()
                .WithAction<ProduceUserRegistrationRequestAction>();

            Guid transformerId = Guid.NewGuid();
            string transformerName = "Registration request to model transformer";

            var transformer = TransformerDefinitionFactory
                .CreateDispatcher()
                .WithParallelismInstances(1)
                .WithCapacity(1)
                .WithBlockInfo(transformerId, transformerName)
                .ThatTransforms<UserRegistrationRequest, UserRegistrationModel>()
                .WithAction<TransformRegistrationRequestToModelAction>();

            Guid batcherId = Guid.NewGuid();
            string batcherName = "Registration model batcher";

            var batcher = BatcherDefinitionFactory
                .CreateDispatcher()
                .WithParallelismInstances(1)
                .WithCapacity(1)
                .WithMaxBatchSize(10)
                .WithMaxBatchTimeout(TimeSpan.FromMinutes(1))
                .WithBlockInfo(batcherId, batcherName)
                .ThatBatches<UserRegistrationModel>();

            Guid consumerId = Guid.NewGuid();
            string consumerName = "Registration model batch consumer";

            // Consumer will consume a List<UserRegistrationModel> because the parent block will be a the batcher we just defined
            // The batcher generates a List<UserRegistrationModel> with size 10, as defined by the max batch size

            var consumer = ConsumerDefinitionFactory
                .CreateConsumer()
                .WithParallelismInstances(1)
                .WithCapacity(1)
                .WithBlockInfo(consumerId, consumerName)
                .ThatConsumes<List<UserRegistrationModel>>()
                .WithAction<ConsumeRegistrationModelListAction>();
            #endregion

            #region Linking blocks and adding the producer to the entrypoints
            producer.LinkTo(transformer);
            transformer.LinkTo(batcher);
            batcher.LinkTo(consumer);

            definition.Entrypoints.Add(producer);

            #endregion


            #region Registering the block actions to the IoC

            DependencyRegistrar dependencyRegistrar = new DependencyRegistrar();

            dependencyRegistrar.Register<ProduceUserRegistrationRequestAction>(() => new ProduceUserRegistrationRequestAction());
            dependencyRegistrar.Register<TransformRegistrationRequestToModelAction>(() => new TransformRegistrationRequestToModelAction());
            dependencyRegistrar.Register<ConsumeRegistrationModelListAction>(() => new ConsumeRegistrationModelListAction());

            IBlockActionFactory actionFactory = new BlockActionIocFactory(dependencyRegistrar);
            #endregion

            #region Creating the flow plan factory and engine then starting the flow plan
            FlowPlanFactory planFactory = new FlowPlanFactory(actionFactory);
            IFlowPlan flowPlan = planFactory.GeneratePlan(definition);

            using FlowEngine flowEngine = new FlowEngine(planFactory) { AuditLogger = new AuditLogger() };
            flowEngine.StartFlow(definition);
            #endregion

            Console.ReadKey();
        }
    }
}
