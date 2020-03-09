# Streamliner
A .NET core library that enables the creation of code workflows that isolate responsibilities. 
Streamliner creates a directed acyclic graph which represents the workflow in separate, single responsibility blocks.

### Check the [examples](../examples) branch for example projects.

# Purpose

Streamliner is a library that enables creation of workflows in the form of Producer/Consumer model. It has been
inspired by Microsoft's TPL DataFlow library. Streamliner was created with abstraction in mind, with blocks that support 
any given model.

# What kind of flows does Streamliner support?

Currently Streamliner supports two kinds of flows:

* Streamflow - Unlimited repetitions, runs until cancellation is requested
* Workflow - Fixed iterations

# What kind of blocks does Streamliner support?

Streamliner supports a number of standard blocks. The blocks supported are listed below:

* Producer (Dispatcher/Broadcaster)
* Transformer (Dispatcher/Broadcaster)
* Batcher (Dispatcher/Broadcaster)
* Waiter (Dispatcher/Broadcaster)
* Consumer

Each block that produces data can either be a dispatcher or a broadcaster. A dispatcher will only send the produced data
to one or more linked blocks conditionally. A broadcaster will broadcast the produced data to all underlying blocks no matter what.

# How does Streamliner work in short?

The top level component of Streamliner is a the FlowEngine component. FlowEngine holds information about all FlowPlan instances. 
FlowEngine can contain as many FlowPlans as the programmer desires. 

A FlowPlan includes all flow block definitions. It knows what blocks are in the plan and it is used to add blocks internally. 
FlowPlan also creates the links between blocks as instructed by the block definitions.

Starting a FlowPlan will trigger all Entrypoint blocks (they are always Producers). In addition, a programmer can trigger a specific Producer
of the FlowPlan by using the Trigger<T>() method and providing the Producer Id and the model with which to start.

# Creating a flow with Streamliner

Streamliner is written with the Fluent design. This means you can create instances of blocks and plans by chaining methods.

Define your flow's metadata. The information required is a Guid Id and string Name.

```csharp
Guid flowId = Guid.NewGuid();
string name = "Test Flow";
```

Optionally, inherit and implement the `ILogger` interface to enable logging. You can also inherit and implement the `IFlowAuditLogger` interface to enable
audit logging, such as receiving notifications about a block's status, if it's starting, started etc. A block will also emit a ping every 30 seconds
to notify the programmer that it is running.

Instantiate a `FlowDefinition` class by using the `FlowDefinitionFactory` fluent class like the example blow.

## For a streamflow:

```csharp
FlowDefinition definition = FlowDefinitionFactory
        .CreateStreamflow()
        .WithServiceInfo(flowId, name);
```

This will create a FlowDefinition that will run until cancellation is requested by the programmer. 

## For a workflow:

```csharp
FlowDefinition definition = FlowDefinitionFactory
        .CreateWorkflow()
        .WithIterations(10)
        .WithServiceInfo(flowId, name);
```

This will create a FlowDefinition that will run for 10 iterations.

**Once you're done with the FlowDefinition, follow the next steps to add blocks to your plan.**

# Creating your plan's blocks

## Producer block

Create a producer as follows:

```csharp
Guid producerId = Guid.NewGuid();
string producerName = "Test Producer";

var producer = ProducerDefinitionFactory
    .CreateDispatcher()
    .WithParallelismInstances(1)
    .WithServiceInfo(producerId, producerName)
    .ThatProduces<HelloWorldModel>()
    .WithAction<TestProducerAction>();
```

**Creating a specific producer type:**

Dispatcher | Broadcaster
------------ | -------------
`CreateDispatcher()` | `CreateBroadcaster()`

In the example above, we're using `CreateDispatcher()` to create a dispatcher producer.

**Fluent method description:**

Method | Description
------------ | -------------
`WithParallelismInstances(uint param)` | Dictates to the plan engine that param number of block instances should be created.
`WithServiceInfo(Guid id, string name)` | Assigns an id and a name to the block. These data are used for identification and logging.
`ThatProduces<T>()` | `T` is the output model type of the producer.
`WithAction<T>()` | T is a class inherited from `ProducerBlockActionBase<T>`. The main action the producer will execute in each cycle.


### Creating the producer's action

Create a class that inherits and implements `ProducerBlockActionBase<T>`. `T` is the model that will be produced by the producer defined above.

Following the previous step, we'll create a model called "HelloWorldModel" and a producer action called "TestProducerAction":

```csharp
public class HelloWorldModel
{
	public string Message { get; set; }
	public int OneNumber { get; set; }
}
```

Our derived `ProducerBlockActionBase<T>` class should look like this:

```csharp
using System;
using System.Threading;
using Streamliner.Actions;

public class TestProducerAction : ProducerBlockActionBase<HelloWorldModel>
{
	public override bool TryProduce(out HelloWorldModel model, CancellationToken token = default(CancellationToken))
	{
		model = new HelloWorldModel()
		{
			Message = "Hello world! We're inside a producer!",
			OneNumber = 20
		};

		return true;
	}
}
```

`TryProduce()` is the main method which determines how the producer is going to produce data. You can fetch data from a queue, a database or
any data source of your choice. The produced model is assigned to the `out T model` parameter.

**Return value:**

Value | Description
------------ | -------------
`true` | Notifies the flow engine that the produced value is to be passed to the next block.
`false` | Notifies the flow engine that the produced value is to be skipped and discarded.

## Transformer block

Create a transformer as follows:

```csharp
Guid transformerId = Guid.NewGuid();
string transformerName = "Test Transformer";

var transformer = TransformerDefinitionFactory
    .CreateDispatcher()
    .WithParallelismInstances(1)
    .WithServiceInfo(transformerId, transformerName)
    .ThatTransforms<HelloWorldModel, NewHelloWorldModel>()
    .WithAction<TestTransformerAction>();
```

**Creating a specific transformer type:**

Dispatcher | Broadcaster
------------ | -------------
`CreateDispatcher()` | `CreateBroadcaster()`

In the example above, we're using `CreateDispatcher()` to create a dispatcher transformer.

**Fluent method description:**

Method | Description
------------ | -------------
`WithParallelismInstances(uint param)` | Dictates to the plan engine that param number of block instances should be created.
`WithServiceInfo(Guid id, string name)` | Assigns an id and a name to the block. These data are used for identification and logging.
`ThatTransforms<TIn, TOut>()` | `TIn` is the input model type of the transformer. `TOut` is the output model type of the transformer. 
`WithAction<T>` | T is a class inherited from `TransformerBlockActionBase<TIn, TOut>`. The main action the transformer will execute in each cycle.

### Creating the transformer's action

Create a class that inherits and implements `TransformerBlockActionBase<TIn, TOut>`. `TIn` is the input model of the transformer, whereas, `TOut` is the output model of the transformer.

In the example where we created a producer, we introduced the `HelloWorldModel`. In this step, we'll create a model called "NewHelloWorldModel" and a transformer action called "TestTransformerAction":

```csharp
public class NewHelloWorldModel
{
	public string Message { get; set; }
	public int OneNumber { get; set; }
	public int SecondNumber { get; set; }
	public Guid AndAGuid { get; set; }
}
```

Our derived `TransformerBlockActionBase<T>` class should look like this:

```csharp
using System;
using System.Threading;
using Streamliner.Actions;

public class TestTransformerAction : TransformerBlockActionBase<HelloWorldModel, NewHelloWorldModel>
{
	public override bool TryTransform(HelloWorldModel input, out NewHelloWorldModel model, CancellationToken token = default(CancellationToken))
	{
		model = new NewHelloWorldModel()
		{
			Message = input.Message,
			OneNumber = input.OneNumber,
			SecondNumber = input.OneNumber * 2,
			AndAGuid = Guid.NewGuid()
		};

		return true;
	}
}
```

`TryTransform()` is the main method which determines how the transformer is going to transform data. You can implement any logic you want inside the method. `TIn input` parameter refers to the model the transformer receives from the producer above. The transformed model is assigned to the `out TOut model` parameter.

**Return value:**

Value | Description
------------ | -------------
`true` | Notifies the flow engine that the transformed value is to be passed to the next block.
`false` | Notifies the flow engine that the transformed value is to be skipped and discarded.

## Batcher block

Create a batcher as follows:

```csharp
Guid batcherId = Guid.NewGuid();
string batcherName = "Test Batcher";

var batcher = BatcherDefinitionFactory
    .CreateDispatcher()
    .WithParallelismInstances(1)
    .WithCapacity(1)
    .WithMaxBatchSize(10)
    .WithMaxBatchTimeout(TimeSpan.FromSeconds(30))
    .WithServiceInfo(batcherId, batcherName)
    .ThatBatches<NewHelloWorldModel>();
```

**Creating a specific batcher type:**

Dispatcher | Broadcaster
------------ | -------------
`CreateDispatcher()` | `CreateBroadcaster()`

In the example above, we're using `CreateDispatcher()` to create a dispatcher batcher.

**Fluent method description:**

Method | Description
------------ | -------------
`WithParallelismInstances(uint param)` | Dictates to the plan engine that param number of block instances should be created.
`WithServiceInfo(Guid id, string name)` | Assigns an id and a name to the block. These data are used for identification and logging.
`WithMaxBatchSize(int maxBatchSize)` | The amount of objects to be batched into a list.
`WithMaxBatchTimeout(TimeSpan timeout)` | The amount of time to wait before sending the batch regardless. If the batch expires, a `List<T>` will be sent to the next blocks regardless.
`ThatBatches<T>()` | `T` is the model type that will be batched. The output is always `List<T>`

**Notes:**  
A batcher receives a `T` model from the previous blocks, packs them into a `List<T>` and sends the list to the next blocks. 
The batcher will send the batch when either the max batch size is collected or the timeout is reached. 
In any case, the maximum amount of items that are produced are determined by the max batch size. 
Setting the batcher's capacity will determine how many `List<T>` it can produce simultaneously. 
A batcher has no underlying action similar to producers, transformers or consumers because the functionality 
is predetermined and is always standard. If you want to apply batching logic, you can implement it in any block that transmits data to a batcher.

## Waiter block

Create a waiter as follows:

```csharp
Guid waiterId = Guid.NewGuid();
string waiterName = "Test Waiter";

var waiter = WaiterDefinitionFactory
    .CreateDispatcher()
    .WithParallelismInstances(1)
    .WithCapacity(1)
    .WithServiceInfo(waiterId, waiterName)
    .ThatWaits<WaitableModel>();
```

**`WaitableModel()` is a model that implements the `IWaitable` interface.**  
[The `IWaitable` interface is the one provided here.](../master/Streamliner/Blocks/Base/IWaitable.cs)

When constructing your WaitableModel, which inherits IWaitable, do it as follows:

```csharp
var waitableModel = new WaitableModel(TimeSpan.FromSeconds(30)) { Properties... };
```

**Creating a specific waiter type:**

Dispatcher | Broadcaster
------------ | -------------
`CreateDispatcher()` | `CreateBroadcaster()`

In the example above, we're using `CreateDispatcher()` to create a dispatcher batcher.

**Fluent method description:**

Method | Description
------------ | -------------
`WithParallelismInstances(uint param)` | Dictates to the plan engine that param number of block instances should be created.
`WithServiceInfo(Guid id, string name)` | Assigns an id and a name to the block. These data are used for identification and logging.
`ThatWaits<T>()` | `T` is the model type that will be waited. `T` must always implement `IWaitable`. See above for more details.

**Notes:**  

A waiter receives a `T` model from the previous blocks and waits for a given `TimeSpan` amount of time before sending the model 
to the following blocks. A waiter has no underlying action similar to producers, transformers or consumers because the 
functionality is predetermined and is always standard. The only required is the `TimeSpan WaitFor { get; set; }` 
property that is defined in the `IWaitable` interface to be defined. 

## Consumer block

Create a consumer as follows:

```csharp
Guid consumerId = Guid.NewGuid();
string consumerName = "Test Consumer";

var consumer = ConsumerDefinitionFactory
    .CreateConsumer()
    .WithParallelismInstances(1)
    .WithServiceInfo(consumerId, consumerName)
    .ThatConsumes<NewHelloWorldModel>()
    .WithAction<TestConsumerAction>();
```

**Fluent method description:**

Method | Description
------------ | -------------
`CreateConsumer()` | Instructs the ConsumerDefinitionFactory to create a new consumer.
`WithParallelismInstances(uint param)` | Dictates to the plan engine that param number of block instances should be created.
`WithServiceInfo(Guid id, string name)` | Assigns an id and a name to the block. These data are used for identification and logging.
`ThatConsumes<T>()` | `T` is the model type that will be consumed.
`WithAction<T>` | T is a class inherited from `ConsumerBlockActionBase<T>`. The main action the consumer will execute in each cycle.

### Creating the consumer's action

Create a class that inherits and implements `ConsumerBlockActionBase<T>`. `T` is the model that will be consumed by the consumer defined above.

Our derived `ConsumerBlockActionBase<T>` class should look like this:

```csharp
using System;
using System.Threading;
using Streamliner.Actions;

public class TestConsumerAction : ConsumerBlockActionBase<NewHelloWorldModel>
{
    public override void Consume(NewHelloWorldModel model, CancellationToken token = new CancellationToken())
    {
        Console.WriteLine($"Message: {model.Message} - One Number: {model.OneNumber} - SecondNumber: {model.SecondNumber} - And a GUID: {model.AndAGuid:N}");
    }
}
```

`Consume()` is the main method which determines how the comsumer is going to consume data. 
You can consume data in any way you determine. For instance, you can store the received data to a database, 
add it to a queue etc. The consumer action does not return a bool since we there are no following blocks and therefore, 
determine whether to proceed or not is unnecessary at this point.

# Linking your plan's blocks together

Blocks can be linked in any way the programmer desires. The only restriction is matching the producing and consuming models of each block. For instance, a block that produces a model of type `TypeOne` can only be linked with a block that consumes `TypeOne`. 

#### Blocks that produce data
* Producer
* Transformer
* Batcher
* Waiter

#### Blocks that consume data
* Transformer
* Batcher
* Waiter
* Consumer

Assuming we have a transformer that consumes a `TConsumed` type object and produces a `TTransformed` type object, we can only link it with a block that consumes a `TTransformed` type object.  
  
Using the blocks created in the example above, we can link them like so:

```csharp
producer.LinkTo(transformer);
transformer.LinkTo(waiter);
waiter.LinkTo(consumer);
```

**Note: A batcher can only be linked with a block that consumes a `List<T>` where `T` is the type that is batched by the batcher.**

**Additionally, blocks can be linked to multiple blocks.**

#### Linking blocks with condition
Block linking can be filtered by using the second parameter of the `LinkTo()` method. The second parameter of `LinkTo()` is a `Func<T, bool>` delegate that takes a `T` parameter and returns `true` when linking should be done and `false` when linking shouldn't.  

For example:

```csharp
producer.LinkTo(transformer, x => x.Message == "Hello world!");
```
This will ensure that that producer will only send models to the transformer when the `string Message { get; set; }` property has a value 
equal to `"Hello world!"`. Use the filter parameter when you want to always filter out specific models from being sent. 
Filtering can also be done inside the block's main action but certain blocks, such as the waiter block or the batcher do 
not have actions to determine what they do when they run.

# Final steps - bootstrapping your flow

## Adding flow plan entrypoints
The entrypoints of each block are producers. In the first step, we created our flow plan definition using the following code:

```csharp
FlowDefinition definition = FlowDefinitionFactory
	.CreateWorkflow()
	.WithIterations(10)
	.WithServiceInfo(flowId, name);
```

After defining the producers, they must be added as entrypoints using the following statement:

```csharp
definition.AddEntrypoint(producer);
```
If you have multiple producers, they must be all added as entrypoints using the above statement.

## Registering the block actions on the internal IoC container

Streamliner has an internal IoC container which is called `DependencyRegistrar`. 
Instantiate a new `DependencyRegistrar` and register the block actions as follows:

```csharp
DependencyRegistrar registrar = new DependencyRegistrar();
registrar.Register<TestProducerAction>(() => new TestProducerAction());
registrar.Register<TestTransformerAction>(() => new TestTransformerAction());
registrar.Register<TestConsumerAction>(() => new TestConsumerAction());
```
Use the concrete type inside the generic parameter of the `Register<T>` method. 

## Creating your flow engine and starting the flow plan

#### Define the block action factory
First, we need to instantiate an `IBlockActionFactory` instance. The block action factory provides the underlying engine a way to retrieve the block actions from an IoC container.

Streamliner has a concrete block action factory which is derived from `IBlockActionFactory`.
```BlockActionIoCFactory``` has a dependency on ```DependencyRegistrar```. If you wish to use
your own IoC container, inherit  ```IBlockActionFactory``` and implement its methods (see below).

```csharp 
IBlockActionFactory actionFactory = new BlockActionIocFactory(registrar);
```
**You can take a look at the ```IBlockActionFactory``` interface [here](../master/Streamliner/Actions/IBlockActionFactory). Use each method of the interface to resolve the action from an IoC container and therefore, you can use any Ioc of your choice.**

#### Define the flow plan factory

Define the flow plan factory with the following statement:

```cshasp
 FlowPlanFactory factory = new FlowPlanFactory(actionFactory);
```

The flow plan factory generates instances of flow plans using `FlowDefinition` to determine their structure.

#### Define the flow engine and start the flow plan

Define the flow engine with the following statement:

```csharp
 FlowEngine engine = new FlowEngine(factory);
```
Additionally, if you have inherited and implemented either or both the `IFlowAuditLogger` and `ILogger` interfaces, you can pass their instances using the `AuditLogger` and `Logger` properties as follows:

```csharp
 FlowEngine engine = new FlowEngine(factory) { AuditLogger = auditLogger, Logger = logger };
```
You do not need to implement both loggers. The flow engine can accept either both or one of the logging types.
Generally, we use the audit logger to monitor each block individually and its status and the regular logger to log events using external services such as LogEntries or a file-based logger. 

#### Start the flow plan
To start a flow plan, use the following statement:

```csharp
engine.StartFlow(definition);
```
The ```definition``` parameter is the instance of ```FlowDefinition``` we have been constructing above. 
A single FlowEngine can start as many flow plans as the programmer desires. It is advised to use separate 
flow engines for flows that handle separate domain data. For instance, a user management service could use 
a single flow engine to handle user registrations, password recoveries etc.

# Contributing

## Found an issue?

Please report any issues you have found by [creating a new issue](https://github.com/Codeh4ck/Streamliner/issues). We will review the case and if it is indeed a problem with the code, I will try to fix it as soon as possible. I want to maintain a healthy and bug-free standard for our code. Additionally, if you have a solution ready for the issue please submit a pull request. 

## Submitting pull requests

Before submitting a pull request to the repository please ensure the following:

* Your code follows the naming conventions [suggested by Microsoft](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines)
* Your code works flawlessly, is fault tolerant and it does not break the library or aspects of it
* Your code follows proper object oriented design principles. Use interfaces!

Your code will be reviewed and if it is found suitable it will be merged. Please understand that the final decision always rests with me. By submitting a pull request you automatically agree that I hold the right to accept or deny a pull request based on my own criteria.

## Contributor License Agreement

By contributing your code to Streamliner you grant Nikolas Andreou a non-exclusive, irrevocable, worldwide, royalty-free, sublicenseable, transferable license under all of Your relevant intellectual property rights (including copyright, patent, and any other rights), to use, copy, prepare derivative works of, distribute and publicly perform and display the Contributions on any licensing terms, including without limitation: (a) open source licenses like the MIT license; and (b) binary, proprietary, or commercial licenses. Except for the licenses granted herein, You reserve all right, title, and interest in and to the Contribution.

You confirm that you are able to grant us these rights. You represent that you are legally entitled to grant the above license. If your employer has rights to intellectual property that you create, You represent that you have received permission to make the contributions on behalf of that employer, or that your employer has waived such rights for the contributions.

You represent that the contributions are your original works of authorship and to your knowledge, no other person claims, or has the right to claim, any right in any invention or patent related to the contributions. You also represent that you are not legally obligated, whether by entering into an agreement or otherwise, in any way that conflicts with the terms of this license.

Nikolas Andreou acknowledges that, except as explicitly described in this agreement, any contribution which you provide is on an "as is" basis, without warranties or conditions of any kind, either express or implied, including, without limitation, any warranties or conditions of title, non-infringement, merchantability, or fitness for a particular purpose.