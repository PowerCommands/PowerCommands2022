using PainKiller.AzureKeyVault.DomainObjects;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Configuration;

public class PowerCommandsConfiguration : BasicCommandsConfiguration
{
    private static PowerCommandsConfiguration? _instance;
    public static PowerCommandsConfiguration? Instance
    {
        get
        {
            if(_instance != null) return _instance;
            _instance = ConfigurationManager.Get<PowerCommandsConfiguration>().Configuration;
            return _instance;

        }
    }
    public KeyVaultConfig KeyVault { get; set; } = new();
    public BaseComponentConfiguration MyExampleCommand { get; set; } = new();

}