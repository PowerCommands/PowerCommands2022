﻿using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class EncryptionCommand : EncryptionCommandBase
{
    public EncryptionCommand(string identifier, BasicCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var encrypt = EncryptString(input.Arguments.First());
        Console.WriteLine(encrypt);
        return new(this, input, $"Input encrypted: {encrypt}", RunResultStatus.Ok);
    }


}