using System;
using ZeroPass.Model.Logging;

namespace ZeroPass.Fakes
{
    public class LoggerFake : ISharedLogger
    {
        public class DummyDisposable : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        public IDisposable BeginScope(string format, params object[] parameters)
            => new DummyDisposable();

        public void LogError(string message)
        {
        }

        public void LogError(Exception exception, string message)
        {
        }

        public void LogError(string format, params object[] parameters)
        {
        }

        public void LogInformation(string message)
        {
        }

        public void LogInformation(string format, params object[] parameters)
        {
        }

        public void LogWarning(string message)
        {
        }

        public void LogWarning(Exception exception, string message)
        {
        }

        public void LogWarning(string format, params object[] parameters)
        {
        }
    }
}
