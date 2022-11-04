﻿using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.WindowsCommands.Commands;

[PowerCommandDesign(description: "Open a given directory or current working folder if argument is omitted",
    arguments: "<directory name>",
    quotes: "<directory name>",
    example: "dir|dir C:\\repos|dir \"C:\\repos\"")]
public class DirCommand : CommandBase<CommandsConfiguration>
{
    public DirCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var directory = string.IsNullOrEmpty(Input.Path) ? AppContext.BaseDirectory : Input.Path;
        if (!string.IsNullOrEmpty(Input.SingleArgument)) directory = Input.SingleArgument;
        if (!string.IsNullOrEmpty(Input.SingleQuote)) directory = Input.SingleQuote;
        if (!Directory.Exists(directory)) return BadParameterError($"Could not find directory \"{directory}\"");
        ShellService.Service.OpenDirectory(directory);
        WriteLine($"Open directory {directory}");
        return Ok();
    }
}