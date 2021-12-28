namespace Streamliner.Core.Utilities
{
    public interface IRunnable
    {
        void Start(object context = null);
        void Stop();

        bool IsRunning { get; }
    }
}
