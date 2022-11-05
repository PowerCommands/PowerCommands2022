using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class CommandTestItem
{
    public string Identifier { get; set; } = "";
    public bool Disabled { get; set; }
    public string Test { get; set; } = "";
    public bool ExpectedResult { get; set; }
    public RunResultStatus Status { get; set; }
    public string Success => (ExpectedResult == (Status == RunResultStatus.Ok || Status == RunResultStatus.Quit)) ? "*YES*" : "*NO*";
}