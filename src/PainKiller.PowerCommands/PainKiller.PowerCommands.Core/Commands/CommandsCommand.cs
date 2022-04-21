using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand( description:      "Shows all custom commands (all if * is used as argument), or filter commands by name or by tag, a tag is something that the command support",
               arguments:        "type: Tag or name filtering, default is tag if no type is given, if * is used all commands will be shown",
               qutes:            "filter: The search parameter",
               defaultParameter: "tag",
               example:          "commands\ncommands *\ncommands name \"encrypt\"\ncommands tag \"checksum\"")]
[Tags("core|help")]
public class CommandsCommand : CommandBase<CommandsConfiguration>
{
    public CommandsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "*") return NoFilter(input);
        if (string.IsNullOrEmpty(input.SingleQuote))
        {
            WriteLine("- All custom commands (use * as argument to also se built in commands):\n", false);
            foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => !c.HasTag("core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
            return CreateRunResult(this, input, RunResultStatus.Ok);
        }
        if (input.SingleArgument == "name") return FilterByName(input);
        return FilterByTag(input);
    }

    private RunResult NoFilter(CommandLineInput input)
    {
        WriteLine($"- All commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private RunResult FilterByName(CommandLineInput input)
    {
        WriteLine($"- Commands with name containing {input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.Identifier.Contains(input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private RunResult FilterByTag(CommandLineInput input)
    {
        WriteLine($"- Commands with tag containing {input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.HasTag(input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}