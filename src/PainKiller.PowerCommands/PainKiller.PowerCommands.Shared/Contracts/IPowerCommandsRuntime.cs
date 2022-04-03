using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IPowerCommandsRuntime
{
    RunResult ExecuteCommand(string rawInput);
}