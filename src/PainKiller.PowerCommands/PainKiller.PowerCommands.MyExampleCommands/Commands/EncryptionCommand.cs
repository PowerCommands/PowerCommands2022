using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class EncryptionCommand : EncryptionCommandBase
{
    public EncryptionCommand(string identifier) : base(identifier) { }

    public override RunResult Run(string input)
    {
        var decrypt = DecryptString(input);
        Console.WriteLine(decrypt);
        return new RunResult {Status = RunResultStatus.Ok};
    }


}