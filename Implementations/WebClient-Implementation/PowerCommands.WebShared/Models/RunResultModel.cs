using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PowerCommands.WebShared.Models;

public class RunResultModel
{
    public CommandLineInput Input { get; set; } = new();
    public string Output { get; set; } = "";
    public RunResultStatus Status { get; set; }
}