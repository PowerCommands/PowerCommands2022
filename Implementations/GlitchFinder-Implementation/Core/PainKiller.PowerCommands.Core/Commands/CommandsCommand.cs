﻿namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandTest(tests: " |--this|--reserved|\"encrypt\"|--default")]
[PowerCommandDesign( description: "Shows commands, or filter commands by name, create a new command, show default command with option --default",
                          quotes: "<filter>",
                           options: "this|reserved|default|!update",
                         example: "//Show all commands|commands|//Show your custom commands|commands --this|//Show reserved commands|commands --reserved|//Search for commands matching \"encrypt\"|commands \"encrypt\"|//Show default command|commands --default|//Update the dir command (command must exist in the configured PowerCommands project)|commands --update dir")]
public class CommandsCommand : CommandBase<CommandsConfiguration>
{
    public CommandsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        Input.DoBadOptionCheck(this);
        if (Input.HasOption("this")) return Custom();
        if (Input.HasOption("default")) return Default();
        if (Input.HasOption("reserved")) return Reserved();
        if (Input.HasOption("update")) return Update();
        if (!string.IsNullOrEmpty(Input.SingleQuote)) return FilterByName();
        return NoFilter();
    }

    private RunResult Update()
    {
        var commandName = Input.GetOptionValue("update");
        if (!DialogService.YesNoDialog($"The command [{commandName}] will be overwritten, continue with update?")) return Ok();
        GithubService.Service.DownloadCommand(commandName);
        return Ok();
    }
    private RunResult NoFilter()
    {
        WriteHeadLine($"\n- All commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands!) WriteLine(consoleCommand.Identifier);
        Console.WriteLine("commands --custom");
        WriteHeadLine($"\nUse --reserved to only show core commands.");
        Console.WriteLine("commands --reserved");
        Console.WriteLine();

        WriteHeadLine($"\nUse describe command to display details about a specific command, for example");
        Console.WriteLine("describe exit");
        WriteHeadLine($"You could also use the --help option for the same thing, but the help option could show something else if it is overriden by the Command author.");
        Console.WriteLine("exit --help");
        return Ok();
    }
    private RunResult Reserved()
    {
        WriteHeadLine($"\n- Reserved commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.GetType().FullName!.StartsWith("PainKiller.PowerCommands.Core"))!) WriteLine(consoleCommand.Identifier);
        WriteHeadLine("\nReserved names should not be used for your custom commands.");
        return Ok();
    }
    private RunResult Custom()
    {
        WriteHeadLine($"\n- custom commands:");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => !c.GetType().FullName!.StartsWith("PainKiller.PowerCommands.Core"))!) WriteLine(consoleCommand.Identifier);
        return Ok();
    }
    private RunResult FilterByName()
    {
        WriteHeadLine($"\n- Commands with name containing {Input.SingleQuote}:\n");
        AppendToOutput = false;
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.Identifier.Contains(Input.SingleQuote))!) WriteLine(command.Identifier);
        AppendToOutput = true;
        return Ok();
    }
    private RunResult Default()
    {
        WriteHeadLine($"\nDefault command:");
        WriteLine(Configuration.DefaultCommand);
        return Ok();
    }
}