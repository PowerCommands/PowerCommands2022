using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand( description:      "Shows all commands, or filter commands by name or by tag, a tag is something that the command support",
               arguments:        "type: Tag or name filtering, default is tag if no type is given",
               qutes:            "filter: The search parameter",
               defaultParameter: "tag",
               example:          "commands name \"encrypt\"\ncommands tag \"checksum\"")]
[Tags("core|help")]
public class CommandsCommand : CommandBase<CommandsConfiguration>
{
    public CommandsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleQuote))
        {
            WriteLine("All commands (No filter quote parameter provided)", false);
            foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands!) WriteLine(consoleCommand.Identifier, addToOutput: false);
            return CreateRunResult(this, input, RunResultStatus.Ok);
        }
        if (input.SingleArgument == "name") return FilterByName(input);
        return FilterByTag(input);
    }

    private RunResult FilterByName(CommandLineInput input)
    {
        WriteLine($"Commands with name containing {input.SingleQuote}:");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.Identifier.Contains(input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private RunResult FilterByTag(CommandLineInput input)
    {
        WriteLine($"Commands with tag containing {input.SingleQuote}:");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.HasTag(input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}