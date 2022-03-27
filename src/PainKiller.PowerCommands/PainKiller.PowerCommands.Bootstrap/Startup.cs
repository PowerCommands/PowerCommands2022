using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Bootstrap.Configuration;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.SerilogExtensions.Managers;

namespace PainKiller.PowerCommands.Bootstrap
{
    public static class Startup
    {
        private static PluginManager<PowerCommandsConfiguration>? _pluginManager;

        public static BaseCommandsConfiguration Initialize()
        {
            var configuration = ConfigurationManager.GetConfiguration<PowerCommandsConfiguration>().Configuration;
            var logger = GetLoggerManager.GetFileLogger(configuration.Log.FileName.GetSafePathRegardlessHowApplicationStarted("logs"));
            logger.LogInformation("Program started, configuration read");
            
            _pluginManager = new PluginManager<PowerCommandsConfiguration>(configuration);
            try
            {
                var validatePlugins = _pluginManager.ValidateConfigurationWithPlugins();

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex,"Critical error, program could not start");
                throw;
            }
            return configuration;
        }
    }
}