# Streamliner
A .NET core library that enables the creation of code workflows that isolate responsibilities. 
Streamliner creates a directed acyclic graph which represents the workflow in separate, single responsibility blocks.

## Purpose

Streamliner is a library that enables creation of workflows in the form of Producer/Consumer model. It has been
inspired by Microsoft's TPL DataFlow library. Streamliner was created with abstraction in mind, with blocks that support 
any given model.

## What kind of blocks does Streamliner support?

Streamliner supports a number of standard blocks. The blocks supported are listed below:

* Producer (Dispatcher/Broadcaster)
* Transformer (Dispatcher/Broadcaster)
* Batcher (Dispatcher/Broadcaster)
* Waiter (Dispatcher/Broadcaster)
* Consumer

Each block that produces data can either be a dispatcher or a broadcaster. A dispatcher will only send the produced data
to one or more linked blocks conditionally. A broadcaster will broadcast the produced data to all underlying blocks no matter what.

## How does Streamline work in short?

The top level component of Streamliner is a the FlowEngine component. FlowEngine holds information about all FlowPlan instances. 
FlowEngine can contain as many FlowPlans as the programmer desires. 

A FlowPlan includes all flow block definitions. It knows what blocks are in the plan and it is used to add blocks internally. 
FlowPlan also creates the links between blocks as instructed by the block definitions.

Starting a FlowPlan will trigger all Entrypoint blocks (they are always Producers). In addition, a programmer can trigger a specific Producer
of the FlowPlan by using the Trigger<T>() method and providing the Producer Id and the model with which to start.

## Creating a flow with Streamliner

Streamliner is written with the Fluent design. This means you can create instances of blocks and plans by chaining methods.

Define your flow's metadata. The information required is a Guid Id and string Name.

```csharp
	Guid flowId = Guid.NewGuid();
	string name = "Test Flow";
```

Optionally, inherit and implement the ILogger interface to enable logging. You can also inherit and implement the IAuditLogger interface to enable
audit logging, such as receiving notifications about a block's status, if it's starting, started etc. A block will also emit a ping every 30 seconds
to notify the programmer that it is running.

