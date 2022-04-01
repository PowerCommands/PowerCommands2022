using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IPowerCommandsManager
{
    RunResult ExecuteCommand(string rawInput);
}