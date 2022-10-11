using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Contracts;
namespace PainKiller.PowerCommands.Core.Commands;

[Tags("core|help")]
[PowerCommand( description:      "Shows commands, or filter commands by name or by tag, a tag is something that the command support",
               arguments:        "tag (default)|type|<name>|?|!",
               qutes:            "filter:<filter>",
               flags:            "custom|reserved|name",   
               suggestion:       "tag",
               example:          "commands|commands --custom|commands --reserved|commands --name \"encrypt\"|commands tag \"checksum\"")]
public class CommandsCommand : CommandBase<CommandsConfiguration>
{
    public CommandsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (Input.HasFlag("custom")) return Custom();
        if (Input.HasFlag("reserved")) return Reserved();
        if (string.IsNullOrEmpty(Input.SingleQuote)) return NoFilter();
        if (Input.HasFlag("name")) return FilterByName();
        return FilterByTag();
    }
    private RunResult NoFilter()
    {
        WriteHeadLine($"- All commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        WriteHeadLine($"\nUse --custom flag to only show your custom commands.");
        Console.WriteLine("commands --custom");
        WriteHeadLine($"Use --reserved to only show core commands.");
        Console.WriteLine("commands --reserved");
        Console.WriteLine();

        WriteHeadLine($"Use describe command to display details about a specific command, for example");
        Console.WriteLine("describe exit");
        WriteHeadLine($"You could also use the --help flag for the same thing, but the help flag could show something else if it is overriden by the Command author.");
        Console.WriteLine("exit --help");
        return CreateRunResult();
    }
    private RunResult Reserved()
    {
        WriteHeadLine($"- Reserved commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.HasTag("core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        WriteHeadLine("This names are reserved and should not be used for your custom commands.");
        return CreateRunResult();
    }
    private RunResult Custom()
    {
        WriteHeadLine($"- custom commands:");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => !c.HasTag("core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        return CreateRunResult();
    }
    private RunResult FilterByName()
    {
        WriteHeadLine($"- Commands with name containing {Input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.Identifier.Contains(Input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult();
    }
    private RunResult FilterByTag()
    {
        WriteHeadLine($"- Commands with tag containing {Input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.HasTag(Input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult();
    }
}