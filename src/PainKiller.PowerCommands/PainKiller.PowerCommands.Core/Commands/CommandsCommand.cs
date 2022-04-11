using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(description: "Command shows all command")]
public class CommandsCommand : CommandBase<CommandsConfiguration>
{
    public CommandsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}