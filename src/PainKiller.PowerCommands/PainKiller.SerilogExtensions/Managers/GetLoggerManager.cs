using Serilog;
using Serilog.Events;

namespace PainKiller.SerilogExtensions.Managers
{
    public static class GetLoggerManager
    {
        public static Microsoft.Extensions.Logging.ILogger GetFileLogger(string fileName)
        {
            var log = new LoggerConfiguration()
                .WriteTo.File(fileName, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel:LogEventLevel.Information)
                .CreateLogger();
            return log.ToMicrosoftILoggerImplementation();
        }
    }
}