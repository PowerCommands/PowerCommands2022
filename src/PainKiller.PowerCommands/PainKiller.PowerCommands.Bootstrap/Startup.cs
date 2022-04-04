﻿using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Bootstrap.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.MyExampleCommands;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;

namespace PainKiller.PowerCommands.Bootstrap
{
    public static class Startup
    {
        public static PowerCommandsManager ConfigureServices()
        {
            var services = PowerCommandServices.Service
                .ShowDiagnostic(true)
                .SetLogMinimumLevel(LogLevel.Information)
                .PersistChanges();


            services.Logger.LogInformation("Program started, configuration read");
            
            var componentManager = new ComponentManager<PowerCommandsConfiguration>(services.Configuration, services.Diagnostic);
            try
            {
                var validatePlugins = componentManager.ValidateConfigurationWithComponents();
                if(!validatePlugins) services.Diagnostic.Message("Some of the components did not pass security check...");
                services.Runtime.Initialize();
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