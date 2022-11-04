namespace PainKiller.PowerCommands.KnowledgeDBCommands.Managers;

public class OpenFolderManager : IShellExecuteManager
{
    public void Run(ShellConfigurationItem configuration, string argument) => ShellService.Service.OpenDirectory(argument);
}