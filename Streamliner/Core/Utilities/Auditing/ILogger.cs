using System;

namespace Streamliner.Core.Utilities.Auditing
{
    public interface ILogger
    {
        void LogEvent(LogType type, string message);
        void LogEvent<T>(LogType type, string message);
        void LogEvent(LogType type, string message, Exception ex);
        void LogEvent<T>(LogType type, string message, Exception ex);
        void LogEvent(LogType type, string message, Exception ex, int id);
        void LogEvent<T>(LogType type, string message, Exception ex, int id);
    }
}