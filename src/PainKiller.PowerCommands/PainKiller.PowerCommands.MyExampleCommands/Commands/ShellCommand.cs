﻿using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Shows how to execute a external program in combination with some custom configuration",
    arguments: "Program name: must be defined in the favorites section in the PowerCommandsConfiguration.yaml file",
    example: "shell music|shell games")]
[Tags("example|shell|git|execute|program")]
public class ShellCommand : CommandBase<PowerCommandsConfiguration>
{
    public ShellCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var favorite = Configuration.Favorites.FirstOrDefault(f => f.Name.ToLower() == input.SingleArgument);
        if (favorite == null) return CreateBadParameterRunResult(this, input, "No matching favorite found in configuration file");

        ShellService.Service.Execute(favorite.NameOfExecutable, arguments: "", workingDirectory: "",WriteLine, favorite.FileExtension);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}