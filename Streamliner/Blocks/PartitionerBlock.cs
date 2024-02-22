using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Streamliner.Blocks.Base;
using Streamliner.Core.Links;
using Streamliner.Core.Routing;
using Streamliner.Core.Utilities.Helpers;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Blocks
{
    public sealed class PartitionerBlock<T> : SourceBlockBase<List<T>>, ITargetBlock<List<T>>
    {
        public IBlockLinkReceiver<List<T>> Receiver { get; }

        private readonly int _partitionSize;
        private readonly TimeSpan _maxPartitionTimeout;

        private List<List<T>> _partitions;
        private bool _toggled;
        private Task _partitionTimeoutTask;

        private readonly object _syncRoot;

        public PartitionerBlock(BlockHeader header, IBlockLinkReceiver<List<T>> receiver, LinkRouterBase<List<T>> router,
            FlowPartitionerDefinition<T> definition) : base(header, definition.Settings, router)
        {
            Receiver = receiver;
            
            FlowPartitionerSettings settings = (FlowPartitionerSettings)definition.Settings;

            _partitionSize = settings.PartitionSize;
            _maxPartitionTimeout = settings.MaxPartitionTimeout;

            _partitions = new List<List<T>>();
            _syncRoot = new object();
            _toggled = false;
        }

        protected override Task ProcessItem(CancellationToken token = default)
        {
            IEnumerable<T> item = Receiver.Receive();
            var chunks = EnumerableHelpers.Chunk(item, _partitionSize);
            
            lock (_syncRoot)
            {
                _toggled = true;

                foreach (var chunk in chunks)
                {
                    var chunkList = chunk.ToList();
                    _partitions.Add(chunkList);
                }
                
                RoutePartitions();
            }

            return Task.CompletedTask;
        }

        private void RoutePartitions()
        {
            List<List<T>> items = null;

            lock (_syncRoot)
            {
                _toggled = false;
                items = _partitions;
                _partitions = new List<List<T>>();
            }

            if (items.Count <= 0) return;
            
            foreach (List<T> partition in items)
                Router.Route(partition);
        }

        private async Task TimeoutPartition()
        {
            while (IsRunning)
            {
                await Task.Delay(_maxPartitionTimeout);
                
                if (_toggled)
                    RoutePartitions();
            }
        }

        protected override void OnStart(object context = null)
        {
            base.OnStart(context);
            
            if (_maxPartitionTimeout != default)
                _partitionTimeoutTask = Task.Factory.StartNew(TimeoutPartition);
        }

        protected override void OnStop()
        {
            base.OnStop();
            _partitionTimeoutTask?.Wait(1000);
        }
    }
}