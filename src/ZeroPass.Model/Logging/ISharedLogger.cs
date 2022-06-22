using System;

namespace ZeroPass.Model.Logging
{
    public interface ISharedLogger
    {
        void LogWarning(string message);

        void LogWarning(Exception exception, string message);

        void LogWarning(string format, params object[] parameters);

        void LogInformation(string message);

        void LogInformation(string format, params object[] parameters);

        void LogError(string message);

        void LogError(Exception exception, string message);

        void LogError(string format, params object[] parameters);

        IDisposable BeginScope(string format, params object[] parameters);
    }
}
