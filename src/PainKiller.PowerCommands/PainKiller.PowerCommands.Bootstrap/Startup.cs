using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.SerilogExtensions.Managers;

namespace PainKiller.PowerCommands.Bootstrap
{
    public static class Startup
    {
        private static PluginManager<PowerCommandsConfiguration>? _pluginManager;

        public static PowerCommandsConfiguration Initialize()
        {
            var configuration = PowerCommandsConfiguration.Instance ?? new PowerCommandsConfiguration();
            var logger = GetLoggerManager.GetFileLogger(configuration.Log.FileName.GetSafePathRegardlessHowApplicationStarted("logs"));

            logger.LogInformation("Program started, configuration read");
            
            _pluginManager = new PluginManager<PowerCommandsConfiguration>(configuration);
            try
            {
                var validatePlugins = _pluginManager.ValidateConfigurationWithPlugins();
                if(!validatePlugins) Console.WriteLine("Some of the components did not pass security check...");
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