﻿using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class DecryptCommand : CommandBase<CommandsConfiguration>
{
    public DecryptCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        var decrypt = EncryptionService.Service.DecryptString(input.SingleArgument);
        WriteLine($"Input decryptet: {decrypt}", false);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}