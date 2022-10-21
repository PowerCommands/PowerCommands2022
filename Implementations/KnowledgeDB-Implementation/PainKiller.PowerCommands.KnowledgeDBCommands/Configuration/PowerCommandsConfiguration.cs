using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Configuration;

public class PowerCommandsConfiguration : CommandsConfiguration
{
    public ShellConfigurationItem ShellConfiguration { get; set; } = new();
}