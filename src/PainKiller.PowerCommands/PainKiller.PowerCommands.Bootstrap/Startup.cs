using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.SerilogExtensions.Managers;

namespace PainKiller.PowerCommands.Bootstrap
{
    public static class Startup
    {
        public static PowerCommandsApplication Initialize()
        {
            var configuration = ConfigurationManager.Get<PowerCommandsConfiguration>().Configuration;
            var logger = GetLoggerManager.GetFileLogger(configuration.Log.FileName.GetSafePathRegardlessHowApplicationStarted("logs"));
            var pcm = new PowerCommandsManager<PowerCommandsConfiguration>(configuration);

            logger.LogInformation("Program started, configuration read");
            
            var componentManager = new ComponentManager<PowerCommandsConfiguration>(configuration);
            try
            {
                var validatePlugins = componentManager.ValidateConfigurationWithComponents();
                if(!validatePlugins) Console.WriteLine("Some of the components did not pass security check...");
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex,"Critical error, program could not start");
                throw;
            }
            return new PowerCommandsApplication(pcm, configuration, logger);
        }
    }
}