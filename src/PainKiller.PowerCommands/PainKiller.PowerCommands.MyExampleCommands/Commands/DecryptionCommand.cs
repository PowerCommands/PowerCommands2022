using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class DecryptCommand : EncryptionCommandBase
{
    public DecryptCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration)
    {
    }

    public override RunResult Run(CommandLineInput input)
    {
        var decrypt = DecryptString(input.Arguments.First());
        AddOutput($"Input decryptet: {decrypt}", true);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}