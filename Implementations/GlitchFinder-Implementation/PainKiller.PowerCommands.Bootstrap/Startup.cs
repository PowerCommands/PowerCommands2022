using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.GlitchFinderCommands;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.Bootstrap
{
    public static class Startup
    {
        public static PowerCommandsManager ConfigureServices()
        {
            var services = PowerCommandServices.Service;

            services.Configuration.Environment.InitializeValues();
            services.Logger.LogInformation("Program started, configuration read");

            var componentManager = new ComponentManager<PowerCommandsConfiguration>(services.ExtendedConfiguration, services.Diagnostic);
            try
            {
                var validatePlugins = componentManager.ValidateConfigurationWithComponents();
                if (!validatePlugins)
                {
                    services.Diagnostic.Warning("\nWarning, some of the components has an invalid checksum in the configuration file");
                    services.Diagnostic.Message("If you continuously working with your Commands, that is ok, when you are distribute your application you should update checksum to match does dll files that ju distribute.\nYou could use the ChecksumCommand class in MyExampleCommands on github to calculate the checksum(MDE5Hash).");
                }
            }
            catch (Exception ex)
            {
                services.Logger.LogCritical(ex, "Critical error, program could not start");
                throw;
            }
            ConsoleService.WriteLine(nameof(Startup), "\nType commands and hit enter to se available commands, use <command name> --help or describe <search> to display documentation.", null);
            return new PowerCommandsManager(services);
        }
    }
}