using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Streamliner.Blocks.Base;
using Streamliner.Definitions;

namespace Streamliner.Core.Links.Local
{
    internal class LocalLinkFactory : IBlockLinkFactory
    {
        private static LocalLinkFactory _instance;

        public static LocalLinkFactory GetInstance()
        {
            return _instance ??= new LocalLinkFactory();
        }

        private readonly Dictionary<Guid, object> _transports;
        public LocalLinkFactory()
        {
            _transports = new Dictionary<Guid, object>();
        }

        public IBlockLink<T> CreateLink<T>(ISourceBlock<T> source, ITargetBlock<T> target, FlowLinkDefinition<T> linkDefinition)
        {
            BlockingCollection<T> transport = CreateTransport(linkDefinition);
            return new LocalBlockLink<T>(transport, source, target, linkDefinition);
        }

        public IBlockLinkReceiver<T> CreateReceiver<T>(FlowLinkDefinition<T> linkDefinition)
        {
            BlockingCollection<T> transport = CreateTransport(linkDefinition);
            return new LocalLinkReceiver<T>(transport);
        }

        private BlockingCollection<T> CreateTransport<T>(FlowLinkDefinition<T> linkDefinition)
        {
            int capacity = linkDefinition.Target.Settings.Capacity;
            Guid id = linkDefinition.Target.BlockInfo.Id;

            BlockingCollection<T> blockingCollection = null;

            if (!_transports.TryGetValue(id, out object transport))
            {
                blockingCollection = new BlockingCollection<T>(capacity);
                _transports.Add(id, blockingCollection);
            }
            else
                blockingCollection = (BlockingCollection<T>) transport;

            return blockingCollection;
        }
    }
}
