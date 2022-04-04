﻿using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

public class ConfigurationCommand : CommandBase<CommandsConfiguration>
{
    public ConfigurationCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input?.SingleArgument == "create")
        {
            var fileName = ConfigurationManager.SaveChanges(new CommandsConfiguration());
            WriteLine($"A new default file named {fileName} has been created in the root directory");
            return CreateRunResult(this, input, RunResultStatus.Ok);
        }
        return new(this, input!, $"", RunResultStatus.Ok);
    }
}