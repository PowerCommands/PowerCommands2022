namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandTest(tests: " ")]
    [PowerCommandDesign(description: "Clears the console",
                 disableProxyOutput: true)]
    public class ClsCommand(string identifier, CommandsConfiguration configuration) : CommandBase<CommandsConfiguration>(identifier, configuration)
    {
        public override RunResult Run()
        {
            ConsoleService.Service.Clear();
            return Ok();
        }
    }
}