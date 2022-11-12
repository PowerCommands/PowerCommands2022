using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands.Templates;

public class NameCommand : CommandBase<CommandsConfiguration>
{
    public NameCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        //insert your code here
        return Ok();
    }
}