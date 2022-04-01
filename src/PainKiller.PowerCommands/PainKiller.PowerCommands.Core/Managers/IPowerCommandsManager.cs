using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Core.Managers;

public interface IPowerCommandsManager
{
    RunResult ExecuteCommand(string rawInput);
}