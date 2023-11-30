using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Configuration.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.MyExampleCommands;
using PainKiller.PowerCommands.Security.Managers;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Bootstrap;
public static class Startup
{
    private static readonly string SetupFileName = Path.Combine(AppContext.BaseDirectory, ConfigurationGlobals.SetupConfigurationFile);

    public static PowerCommandsManager ConfigureServices()
    {
        if (!File.Exists(SetupFileName))
        {
            var setupConfiguration = new SetupConfiguration { Setup = DateTime.Now, User = Environment.UserName };
            ConfigurationService.Service.Create(setupConfiguration, SetupFileName);

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
        try
        {
            var setupSecret = DialogService.YesNoDialog("Do you want to setup the encryption keys?");
            if (!setupSecret) return;

            ConsoleService.Service.WriteHeaderLine(nameof(InitSecret), "Run as administrator if you intend to use Power Commands as a job running by a service account");
            var serviceAccountUse = DialogService.YesNoDialog("Do you intend to run this application with a service account?");

            var sharedSecret = AESEncryptionManager.GetStrongRandomString();
            var salt = AESEncryptionManager.GetStrongRandomString(desiredByteLength: 16);

            Environment.SetEnvironmentVariable("_encryptionManager", sharedSecret, serviceAccountUse ? EnvironmentVariableTarget.Machine : EnvironmentVariableTarget.User);
            var securityConfig = new SecurityConfiguration { Encryption = new EncryptionConfiguration { SharedSecretEnvironmentKey = ConfigurationGlobals.EncryptionEnviromentVariableName, SharedSecretSalt = salt } };
            var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, ConfigurationGlobals.SecurityFileName);
            if (serviceAccountUse)
            {
                var yaml = securityConfig.GetYaml();
                Console.WriteLine($"You need to add this to the {ConfigurationGlobals.MainConfigurationFile} file, so that a service account or a build in account can use the encryption functionality.");
                ConsoleService.Service.WriteLine(nameof(InitSecret),yaml);
                return;
            }

            ConfigurationService.Service.Create(securityConfig, fileName);
        }
        catch (Exception ex)
        {
            ConsoleService.Service.WriteLine(nameof(InitSecret),$"You may need to run as administrator to perform setup, delete the {SetupFileName} to run this initialization again.");
            Console.WriteLine(ex.ToString());
        }
    }
}