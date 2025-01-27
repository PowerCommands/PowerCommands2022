namespace PainKiller.PowerCommands.Core.Commands
{
    public class CommandTemplate(string identifier, CommandsConfiguration configuration) : CommandBase<CommandsConfiguration>(identifier, configuration)
    {
        public override RunResult Run() => Ok();
    }
}