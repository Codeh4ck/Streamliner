using System;
using System.Threading;
using System.Threading.Tasks;
using Streamliner.Core.Utilities;
using Streamliner.Core.Utilities.Auditing;
using Streamliner.Definitions.Metadata.Blocks;
using Streamliner.Definitions.Metadata.Flow;

namespace Streamliner.Blocks.Base
{
    public abstract class BlockBase : Runnable, IBlock
    {
        public BlockHeader Header { get; }

        private protected FlowSettings FlowSettings { get; }

        public ILogger Logger { get; set; }
        public IFlowAuditLogger AuditLogger { get; set; }

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        private Task[] _processingTasks;
        private Task _pingTask;

        protected BlockBase(BlockHeader header, FlowSettings settings)
        {
            Header = header;
            FlowSettings = settings;
        }

        protected abstract void ProcessItem(CancellationToken token = default(CancellationToken));

        private void Process()
        {
            if (FlowSettings.Type == FlowType.StreamFlow)
                while (!_cancellationTokenSource.IsCancellationRequested)
                    ProcessOnce();
            else
                for(int x = 0; x < FlowSettings.Iterations; x++)
                    ProcessOnce();
        }

        private void ProcessOnce()
        {
            try
            {
                ProcessItem(_cancellationToken);
            }
            catch (Exception ex)
            {
                Logger?.LogEvent(LogType.Error,
                    $"Unhandled exception occurred in block \"{Header.BlockInfo.Name}\" with ID {Header.BlockInfo.Id:N}. " +
                    $"Exception message: {ex.Message}");
            }
        }

        private void ProcessWithAuditing()
        {
            if (FlowSettings.Type == FlowType.StreamFlow)
                while (!_cancellationTokenSource.IsCancellationRequested)
                    ProcessOnceWithAuditing();
            else
                for(int x = 0; x < FlowSettings.Iterations; x++)
                    ProcessOnceWithAuditing();
        }

        private void ProcessOnceWithAuditing()
        {
            Guid cycleId = Guid.NewGuid();
            try
            {
                AuditLogger.BlockProcessing(Header, cycleId);
                ProcessItem(_cancellationToken);
                AuditLogger.BlockProcessed(Header, cycleId);
            }
            catch (Exception ex)
            {
                try
                {
                    AuditLogger.BlockException(Header, cycleId, ex);
                }
                catch
                {
                    // ignored
                }

                Logger?.LogEvent(LogType.Error,
                    $"Unhandled exception occurred in block \"{Header.BlockInfo.Name}\" with ID {Header.BlockInfo.Id:N}. " +
                    $"Exception message: {ex.Message}");
            }
        }

        private void Ping()
        {
            if (FlowSettings.Type == FlowType.StreamFlow)
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    PingOnce();
                    _cancellationToken.WaitHandle.WaitOne(TimeSpan.FromSeconds(30));
                }
            else
                for(int x = 0; x < FlowSettings.Iterations; x++)
                    PingOnce();
        }

        private void PingOnce()
        {
            AuditLogger.BlockPing(Header);
        }

        public virtual void Wait()
        {
            Task.WaitAll(_processingTasks, _cancellationToken);
        }

        protected override void OnStart(object context = null)
        {
            Logger?.LogEvent<BlockBase>(LogType.Info, $"Starting block: {Header.BlockInfo.Name}.");
            AuditLogger?.BlockStarting(Header);

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            StartFlow();

            Logger?.LogEvent<BlockBase>(LogType.Info, $"Started block: {Header.BlockInfo.Name}.");
            AuditLogger?.BlockStarted(Header);
        }

        protected override void OnStop()
        {
            AuditLogger?.BlockStopping(Header);

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

            AuditLogger?.BlockStopped(Header);
        }

        private void StartFlow()
        {
            _processingTasks = new Task[FlowSettings.ParallelismInstances];

            if (AuditLogger != null)
            {
                _pingTask = new Task(Ping);
                _pingTask.Start();
            }

            for (uint x = 0; x < FlowSettings.ParallelismInstances; x++)
            {
                if (AuditLogger == null)
                    _processingTasks[x] = new Task(Process, _cancellationToken, TaskCreationOptions.LongRunning);
                else
                    _processingTasks[x] = new Task(ProcessWithAuditing, _cancellationToken, TaskCreationOptions.LongRunning);

                _processingTasks[x].Start();
            }
        }
    }
}
