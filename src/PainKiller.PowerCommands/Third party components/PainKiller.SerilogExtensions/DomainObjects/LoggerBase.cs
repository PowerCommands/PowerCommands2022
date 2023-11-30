using Microsoft.Extensions.Logging;

namespace PainKiller.SerilogExtensions.DomainObjects
{
    public class LoggerBase(Serilog.ILogger logger) : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var messageTemplate = $"[{Environment.UserName}]\t{formatter.Invoke(state, exception)}";
            var loglevelEvent = logLevel.ToLogLevel();
            logger.Write(loglevelEvent, messageTemplate, state, exception);
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            var loglevelEvent = logLevel.ToLogLevel();
            return logger.IsEnabled(loglevelEvent);
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull { return null; }
    }
}