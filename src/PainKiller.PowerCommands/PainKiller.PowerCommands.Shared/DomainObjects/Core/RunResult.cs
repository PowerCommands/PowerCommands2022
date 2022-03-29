using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class RunResult
{
    public string Output { get; set; }
    public RunResultStatus Status { get; set; }
}