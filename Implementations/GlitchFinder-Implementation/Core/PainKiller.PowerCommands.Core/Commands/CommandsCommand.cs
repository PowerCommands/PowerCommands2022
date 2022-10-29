﻿using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand( description:      "Shows commands, or filter commands by name, create a new command, show default command with flag --default",
               qutes:            "//Search for a specific command with a filter that is using Contains to match loaded commands.|filter",
               flags:            "//Show only this projects available commands.|this|//Show the Core command names that should not be used.|reserved|//The name flag is used when you also use the create flag and must be followed next with the name argument (se examples)|name|//Create a new command, that requires that you are starting the application from Visual Studio or some other IDE like VS Code|new|//Shows the default command configured in the PowerCommandsConfiguration.yaml file|default",
               example:          "//Show all commands|commands|//Show your custom commands|commands --this|//Show reserved commands|commands --reserved|//Search for commands matching \"encrypt\"|commands \"encrypt\"|//Create a new command|commands --create --name MyNewCommand|//Show default command|commands --default")]
public class CommandsCommand : CommandBase<CommandsConfiguration>
{
    public CommandsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        Input.DoBadFlagCheck(this);
        if (Input.HasFlag("this")) return Custom();
        if (Input.HasFlag("default")) return Default();
        if (Input.HasFlag("reserved")) return Reserved();
        if (Input.HasFlag("new") && Input.HasFlag("name")) return Create(Input.GetFlagValue("name"));
        if (!string.IsNullOrEmpty(Input.SingleQuote)) return FilterByName();
        return NoFilter();
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
        return CreateRunResult();
    }
    private RunResult Reserved()
    {
        WriteHeadLine($"\n- Reserved commands:\n");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.GetType().FullName!.StartsWith("PainKiller.PowerCommands.Core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        WriteHeadLine("\nReserved names should not be used for your custom commands.");
        return CreateRunResult();
    }
    private RunResult Custom()
    {
        WriteHeadLine($"\n- custom commands:");
        foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => !c.GetType().FullName!.StartsWith("PainKiller.PowerCommands.Core"))!) WriteLine(consoleCommand.Identifier, addToOutput: false);
        return CreateRunResult();
    }
    private RunResult FilterByName()
    {
        WriteHeadLine($"\n- Commands with name containing {Input.SingleQuote}:\n");
        foreach (var command in IPowerCommandsRuntime.DefaultInstance?.Commands.Where(c => c.Identifier.Contains(Input.SingleQuote))!) WriteLine(command.Identifier, addToOutput: false);
        return CreateRunResult();
    }
    private RunResult Create(string name)
    {
        var templateManager = new TemplateManager(name, WriteLine);
        templateManager.CreateCommand(templateName:"Default", name);
        return CreateRunResult();
    }
    private RunResult Default()
    {
        WriteHeadLine($"\nDefault command:");
        WriteLine(Configuration.DefaultCommand);
        return CreateRunResult();
    }
}