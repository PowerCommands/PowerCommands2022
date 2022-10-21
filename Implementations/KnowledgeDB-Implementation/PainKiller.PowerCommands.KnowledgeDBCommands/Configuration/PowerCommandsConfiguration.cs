using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Configuration;

public class PowerCommandsConfiguration : CommandsConfiguration
{
    public string BackupPath { get; set; } = "C:\\Temp";
    public ShellConfigurationItem ShellConfiguration { get; set; } = new();
}