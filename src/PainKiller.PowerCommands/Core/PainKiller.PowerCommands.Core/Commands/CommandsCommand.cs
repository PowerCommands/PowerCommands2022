using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Contracts;
namespace PainKiller.PowerCommands.Core.Commands;

[Tags("core|help")]
[PowerCommand( description:      "Shows commands, or filter commands by name or by tag, a tag is something that the command support",
               arguments:        "tag (default)|type|<name>|?|!",
               qutes:            "filter:<filter>",
               suggestion:       "tag",
               example:          "commands|commands *|commands !|commands name \"encrypt\"|commands tag \"checksum\"")]
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
        return CreateRunResult(input);
    }
    private RunResult Reserved(CommandLineInput input)
    {
        WriteHeadLine($"- Reserved commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.HasTag("core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        WriteHeadLine("This names are reserved and should not be used for your custom commands.");
        return CreateRunResult(input);
    }
    private RunResult Custom(CommandLineInput input)
    {
        WriteHeadLine($"- custom commands:");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => !c.HasTag("core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        return CreateRunResult(input);
    }
    private RunResult FilterByName(CommandLineInput input)
    {
        WriteHeadLine($"- Commands with name containing {input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.Identifier.Contains(input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult(input);
    }
    private RunResult FilterByTag(CommandLineInput input)
    {
        WriteHeadLine($"- Commands with tag containing {input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.HasTag(input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult(input);
    }
}