using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.KnowledgeDBCommands.Configuration;
using PainKiller.PowerCommands.KnowledgeDBCommands.Contracts;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Managers;

public class OpenFolderManager : IShellExecuteManager
{
    public void Run(ShellConfigurationItem configuration, string argument) => ShellService.Service.OpenDirectory(argument);
}