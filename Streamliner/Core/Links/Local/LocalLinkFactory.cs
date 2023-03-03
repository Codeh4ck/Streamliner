using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Streamliner.Blocks.Base;
using Streamliner.Definitions;

namespace Streamliner.Core.Links.Local;

internal class LocalLinkFactory : IBlockLinkFactory
{
    private static LocalLinkFactory _instance;

    public static LocalLinkFactory GetInstance() => _instance ??= new();

    private readonly Dictionary<Guid, object> _buffers;

    public LocalLinkFactory() => _buffers = new();

    public IBlockLink<T> CreateLink<T>(ISourceBlock<T> source, ITargetBlock<T> target, FlowLinkDefinition<T> linkDefinition)
    {
        BlockingCollection<T> buffer = CreateBuffer(linkDefinition);
        return new LocalBlockLink<T>(buffer, source, target, linkDefinition);
    }

    public IBlockLinkReceiver<T> CreateReceiver<T>(FlowLinkDefinition<T> linkDefinition)
    {
        BlockingCollection<T> buffer = CreateBuffer(linkDefinition);
        return new LocalLinkReceiver<T>(buffer);
    }

    private BlockingCollection<T> CreateBuffer<T>(FlowLinkDefinition<T> linkDefinition)
    {
        int capacity = linkDefinition.Target.Settings.Capacity;
        Guid id = linkDefinition.Target.BlockInfo.Id;

        BlockingCollection<T> blockingCollection;

        if (!_buffers.TryGetValue(id, out object buffer))
        {
            blockingCollection = new(capacity);
            _buffers.Add(id, blockingCollection);
        }
        else
            blockingCollection = (BlockingCollection<T>) buffer;

        return blockingCollection;
    }
}