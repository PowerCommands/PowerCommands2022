using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

public class TodoCommand : CommandBase<CommandsConfiguration>
{
    public TodoCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        //insert your code here
        return Ok();
    }
}