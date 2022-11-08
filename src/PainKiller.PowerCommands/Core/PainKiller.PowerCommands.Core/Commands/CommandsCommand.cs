namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandTest(tests: " |--this|--reserved|\"encrypt\"|--default")]
[PowerCommandDesign( description: "Shows commands, or filter commands by name, create a new command, show default command with flag --default",
                          quotes: "<filter>",
                           flags: "this|reserved|default|!update",
                         example: "//Show all commands|commands|//Show your custom commands|commands --this|//Show reserved commands|commands --reserved|//Search for commands matching \"encrypt\"|commands \"encrypt\"|//Show default command|commands --default")]
public class CommandsCommand : CommandBase<CommandsConfiguration>
{
    public CommandsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        Input.DoBadFlagCheck(this);
        if (Input.HasFlag("this")) return Custom();
        if (Input.HasFlag("default")) return Default();
        if (Input.HasFlag("reserved")) return Reserved();
        if (Input.HasFlag("update")) return Update();
        if (!string.IsNullOrEmpty(Input.SingleQuote)) return FilterByName();
        return NoFilter();
    }

    private RunResult Update()
    {
        var commandName = Input.GetFlagValue("update");
        if (!DialogService.YesNoDialog($"The command [{commandName}] will be overwritten, continue with update?")) return Ok();
        GithubService.Service.DownloadCommand(commandName);
        return Ok();
    }
    private RunResult NoFilter()
    {
        WriteHeadLine($"\n- All commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        WriteHeadLine($"\nUse --custom flag to only show your custom commands.");
        Console.WriteLine("commands --custom");
        WriteHeadLine($"\nUse --reserved to only show core commands.");
        Console.WriteLine("commands --reserved");
        Console.WriteLine();

        WriteHeadLine($"\nUse describe command to display details about a specific command, for example");
        Console.WriteLine("describe exit");
        WriteHeadLine($"You could also use the --help flag for the same thing, but the help flag could show something else if it is overriden by the Command author.");
        Console.WriteLine("exit --help");
        return Ok();
    }
    private RunResult Reserved()
    {
        WriteHeadLine($"\n- Reserved commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.GetType().FullName!.StartsWith("PainKiller.PowerCommands.Core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        WriteHeadLine("\nReserved names should not be used for your custom commands.");
        return Ok();
    }
    private RunResult Custom()
    {
        WriteHeadLine($"\n- custom commands:");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => !c.GetType().FullName!.StartsWith("PainKiller.PowerCommands.Core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        return Ok();
    }
    private RunResult FilterByName()
    {
        WriteHeadLine($"\n- Commands with name containing {Input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.Identifier.Contains(Input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return Ok();
    }
    private RunResult Default()
    {
        WriteHeadLine($"\nDefault command:");
        WriteLine(Configuration.DefaultCommand);
        return Ok();
    }
}