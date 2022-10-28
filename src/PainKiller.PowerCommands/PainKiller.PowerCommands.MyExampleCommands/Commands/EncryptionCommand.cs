using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Encrypt the argument input")]
public class EncryptionCommand : CommandBase<CommandsConfiguration>
{
    public EncryptionCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine(EncryptionService.Service.EncryptString(Input.SingleQuote));
        return CreateRunResult();
    }
}