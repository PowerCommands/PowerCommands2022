namespace PainKiller.PowerCommands.KnowledgeDBCommands.Contracts;

public interface IShellExecuteManager
{
    void Run(ShellConfigurationItem configuration, string argument);
}