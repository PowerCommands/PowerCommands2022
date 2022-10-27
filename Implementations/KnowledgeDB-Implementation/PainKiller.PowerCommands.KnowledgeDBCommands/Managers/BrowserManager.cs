using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.KnowledgeDBCommands.Configuration;
using PainKiller.PowerCommands.KnowledgeDBCommands.Contracts;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Managers;

public class BrowserManager : IShellExecuteManager
{
    public void Run(ShellConfigurationItem configuration, string argument) => ShellService.Service.OpenWithDefaultProgram($"{argument}");
}