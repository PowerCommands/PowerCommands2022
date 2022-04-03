using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class DecryptionCommand : EncryptionCommandBase
{
    public DecryptionCommand(string identifier, BasicCommandsConfiguration configuration) : base(identifier, configuration)
    {
    }

    public override RunResult Run(CommandLineInput input)
    {
        var decrypt = DecryptString(input.Arguments.First());
        AddOutput($"Input decryptet: {decrypt}", true);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}