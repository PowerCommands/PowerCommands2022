namespace PainKiller.PowerCommands.DrontlogCommands.Configuration;

public class PowerCommandsConfiguration : CommandsConfiguration
{
    public string DefaultGitRepositoryPath { get; set; } = "C:\\repos";
    public string ConnectionString { get; set; } = "";
}