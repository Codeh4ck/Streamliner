using System;

namespace Streamliner.Core.Utilities
{
    public abstract class Runnable : IRunnable, IDisposable
    {
        public bool IsRunning => _status == RunnableStatus.Running;

        private RunnableStatus _status = RunnableStatus.Stopped;

        protected abstract void OnStart(object context = null);
        protected abstract void OnStop();

        public void Start(object context = null)
        {
            if (_status == RunnableStatus.Running) return;

            try
            {
                _status = RunnableStatus.Running;
                lock (this)
                {
                    OnStart(context);
                }
            }
            catch (Exception)
            {
                Stop();
                throw;
            }
        }

        public void Stop()
        {
            if (_status == RunnableStatus.Stopped) return;

            try
            {
                _status = RunnableStatus.Stopping;

                lock (this)
                {
                    OnStop();
                }
            }
            finally
            {
                _status = RunnableStatus.Stopped;
            }
        }

        public void Dispose(bool disposing)
        {
            Stop();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
