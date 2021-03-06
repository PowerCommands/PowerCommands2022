using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.DrontlogCommands;
using PainKiller.PowerCommands.DrontlogCommands.Configuration;

namespace PainKiller.PowerCommands.Bootstrap
{
    public static class Startup
    {
        public static PowerCommandsManager ConfigureServices()
        {
            var services = PowerCommandServices.Service;

            services.Logger.LogInformation("Program started, configuration read");
            
            var componentManager = new ComponentManager<PowerCommandsConfiguration>(services.ExtendedConfiguration, services.Diagnostic);
            try
            {
                var validatePlugins = componentManager.ValidateConfigurationWithComponents();
                if(!validatePlugins) services.Diagnostic.Message("Some of the components did not pass security check...");
            }
            catch (Exception ex)
            {
                services.Logger.LogCritical(ex,"Critical error, program could not start");
                throw;
            }
            return new PowerCommandsManager(services);
        }
    }
}