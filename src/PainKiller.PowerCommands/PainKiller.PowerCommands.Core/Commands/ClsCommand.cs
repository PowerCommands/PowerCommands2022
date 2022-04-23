using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(description: "Clears the console",
                  example: "cls")]
[Tags("core|console|clear")]
public class ClsCommand : CommandBase<CommandsConfiguration>
{
    public ClsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        Console.Clear();
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}