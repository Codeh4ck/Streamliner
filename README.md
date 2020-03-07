


# Streamliner
A .NET core library that enables the creation of code workflows that isolate responsibilities. 
Streamliner creates a directed acyclic graph which represents the workflow in separate, single responsibility blocks.

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

# How does Streamline work in short?

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

Optionally, inherit and implement the `ILogger` interface to enable logging. You can also inherit and implement the `IAuditLogger` interface to enable
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

Create a class that inherits and implements `ProducerBlockActionBase<T>`. `T` is the model that will be produced by the produced defined above.

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

### Creating the transformer' action

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