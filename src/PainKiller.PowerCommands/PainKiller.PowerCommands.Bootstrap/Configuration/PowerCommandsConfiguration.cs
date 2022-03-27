using PainKiller.AzureKeyVault.DomainObjects;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Bootstrap.Configuration;

public class PowerCommandsConfiguration : BaseCommandsConfiguration
{
    public KeyVaultConfig KeyVault { get; set; } = new KeyVaultConfig();
}