using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.Enums;
using PainKiller.PowerCommands.Shared.Utils;
using PainKiller.PowerCommands.Shared.Utils.DisplayTable;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class CommandTestItem : IConsoleCommandTable
{

    [ColumnRenderOptions(caption:"Command", order:0 )]
    public string Command { get; set; } = "Missing PowerCommandTest attribute";
    [ColumnRenderOptions(caption: "Disabled", ingnore: true)]
    public bool Disabled { get; set; }
    [ColumnRenderOptions(caption: "Test", order: 1)]
    public string Test { get; set; } = "";
    [ColumnRenderOptions(caption: "Expected", order: 2)]
    public bool ExpectedResult { get; set; }
    [ColumnRenderOptions(caption: "Return status", order: 3)]
    public RunResultStatus Status { get; set; }
    [ColumnRenderOptions(caption: "Result", order: 4,renderFormat:ColumnRenderFormat.SucessOrFailure, trigger1:"*YES*",trigger2:"*NO*",mark:"*")]
    public string Success => (ExpectedResult == (Status == RunResultStatus.Ok || Status == RunResultStatus.Quit)) ? "*YES*" : "*NO*";
}