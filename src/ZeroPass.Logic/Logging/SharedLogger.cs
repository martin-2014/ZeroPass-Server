using Microsoft.Extensions.Logging;
using System;
using ZeroPass.Model.Logging;

namespace ZeroPass.Service.Logging
{
    public class SharedLogger : ISharedLogger
    {
        readonly ILogger Logger;

        public SharedLogger(ILogger<SharedCategory> logger)
            => Logger = logger;

        public IDisposable BeginScope(string format, params object[] parameters)
            => Logger.BeginScope(format, parameters);

        public void LogError(string message)
            => Logger.LogError(message);

        public void LogError(Exception exception, string message)
            => Logger.LogError(exception, message);

        public void LogError(string format, params object[] parameters)
            => Logger.LogError(format, parameters);

        public void LogInformation(string message)
            => Logger.LogInformation(message);

        public void LogInformation(string format, params object[] parameters)
            => Logger.LogInformation(format, parameters);

        public void LogWarning(string message)
            => Logger.LogWarning(message);

        public void LogWarning(Exception exception, string message)
            => Logger.LogWarning(exception, message);

        public void LogWarning(string format, params object[] parameters)
            => Logger.LogWarning(format, parameters);
    }

    public interface SharedCategory
    {
    }
}
