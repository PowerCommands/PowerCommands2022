using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand( description:      "Shows commands, or filter commands by name or by tag, a tag is something that the command support",
               arguments:        "type: Tag, type or name filtering, default is tag if no type is given\nIf ? is used your custom commands will be shown.\nIf ! is used, only the core commands will be shown, this names are reserved and should not be used for custom commands.",
               qutes:            "filter: The search parameter",
               defaultParameter: "tag",
               example:          "commands|commands *|commands !|commands name \"encrypt\"|commands tag \"checksum\"")]
[Tags("core|help")]
public class CommandsCommand : CommandBase<CommandsConfiguration>
{
    public CommandsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "?") return Custom(input);
        if (input.SingleArgument == "!") return Reserved(input);
        if (string.IsNullOrEmpty(input.SingleQuote)) return NoFilter(input);
        if (input.SingleArgument == "name") return FilterByName(input);
        return FilterByTag(input);
    }

    private RunResult NoFilter(CommandLineInput input)
    {
        WriteHeadLine($"- All commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        WriteHeadLine($"\nUse ? to only show your custom commands.");
        WriteHeadLine($"Use ! to only show core commands.");
        Console.WriteLine();

        WriteHeadLine($"Use describe command to display details about a specific command, for example");
        Console.WriteLine("describe exit");
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private RunResult Reserved(CommandLineInput input)
    {
        WriteHeadLine($"- Reserved commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.HasTag("core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        WriteHeadLine("This names are reserved and should not be used for your custom commands.");
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private RunResult Custom(CommandLineInput input)
    {
        WriteHeadLine($"- custom commands:");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => !c.HasTag("core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private RunResult FilterByName(CommandLineInput input)
    {
        WriteHeadLine($"- Commands with name containing {input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.Identifier.Contains(input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private RunResult FilterByTag(CommandLineInput input)
    {
        WriteHeadLine($"- Commands with tag containing {input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.HasTag(input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}