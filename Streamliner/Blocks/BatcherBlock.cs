using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Streamliner.Blocks.Base;
using Streamliner.Core.Links;
using Streamliner.Core.Routing;
using Streamliner.Definitions;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Blocks
{
    public sealed class BatcherBlock<T> : SourceBlockBase<List<T>>, ITargetBlock<T>
    {
        public IBlockLinkReceiver<T> Receiver { get; }

        private readonly int _maxBatchSize;
        private readonly TimeSpan _maxBatchTimeout;

        private List<T> _batch;
        private bool _toggled;
        private Task _batchTimeoutTask;

        private readonly object _syncRoot;

        public BatcherBlock(BlockHeader header, IBlockLinkReceiver<T> receiver,
            LinkRouterBase<List<T>> router, FlowBatcherDefinition<T> definition) : base(header, definition.Settings, router)
        {
            Receiver = receiver;
            
            FlowBatcherSettings settings = (FlowBatcherSettings)definition.Settings;

            _maxBatchSize = settings.MaxBatchSize;
            _maxBatchTimeout = settings.MaxBatchTimeout;

            _batch = new List<T>();
            _syncRoot = new object();
            _toggled = false;
        }

        protected override void ProcessItem(CancellationToken token = default(CancellationToken))
        {
            T item = Receiver.Receive();
            lock (_syncRoot)
            {
                _toggled = true;    
                _batch.Add(item);
                if (_batch.Count >= _maxBatchSize)
                    RouteBatch();
            }
        }

        private void RouteBatch()
        {
            List<T> items = null;

            lock (_syncRoot)
            {
                _toggled = false;
                items = _batch;
                _batch = new List<T>();
            }

            if(items.Count > 0)
                Router.Route(items);
        }

        private void TimeoutBatch()
        {
            while (IsRunning)
            {
                Thread.Sleep(_maxBatchTimeout);
                if (_toggled)
                    RouteBatch();
            }
        }

        protected override void OnStart(object context = null)
        {
            base.OnStart(context);
            
            if (_maxBatchTimeout != default(TimeSpan))
                _batchTimeoutTask = Task.Factory.StartNew(TimeoutBatch);
        }

        protected override void OnStop()
        {
            base.OnStop();
            _batchTimeoutTask?.Wait(1000);
        }
    }
}
