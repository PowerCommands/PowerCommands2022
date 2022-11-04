namespace PainKiller.PowerCommands.KnowledgeDBCommands.Managers;

public class BrowserManager : IShellExecuteManager
{
    public void Run(ShellConfigurationItem configuration, string argument) => ShellService.Service.OpenWithDefaultProgram($"{argument}");
}