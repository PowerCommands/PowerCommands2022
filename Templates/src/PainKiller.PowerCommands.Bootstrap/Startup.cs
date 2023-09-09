using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Core.Services;
using $ext_projectname$Commands;
using PainKiller.PowerCommands.Shared.Contracts;

namespace $safeprojectname$;
public static class Startup
{
    public static PowerCommandsManager ConfigureServices()
    {
        if (!Directory.Exists(ConfigurationGlobals.ApplicationDataFolder))
        {
            Directory.CreateDirectory(ConfigurationGlobals.ApplicationDataFolder);
            InitSecret();
            ConsoleService.Service.WriteSuccessLine(nameof(Startup), "\nFirst startup basic application configuration completed...");
            ConsoleService.Service.WriteSuccessLine(nameof(Startup), "You will need to restart the application before the changes take effect.");
        }
        var services = PowerCommandServices.Service;
        
        services.Configuration.Environment.InitializeValues();
        services.Logger.LogInformation("Program started, configuration read");

        ConsoleService.Service.WriteLine(nameof(Startup), "\nUse the tab key to cycle trough available commands, suggestions and options.\nUse <command name> --help or describe <search phrase> to display documentation.", null);
        ConsoleService.Service.WriteLine(nameof(Startup), "\nUse up or down key  to cycle trough command history.", null);
        return new PowerCommandsManager(services);
    }

    private static void InitSecret()
    {
        var firstHalf = IEncryptionService.GetRandomSalt();;
        var secondHalf = IEncryptionService.GetRandomSalt();;
        Environment.SetEnvironmentVariable("_encryptionManager", firstHalf, EnvironmentVariableTarget.User);
        var securityConfig = new SecurityConfiguration { Encryption = new EncryptionConfiguration { SharedSecretEnvironmentKey = "_encryptionManager", SharedSecretSalt = secondHalf } };
        var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, ConfigurationGlobals.SecurityFileName);
        ConfigurationService.Service.Create(securityConfig, fileName);
    }
}
