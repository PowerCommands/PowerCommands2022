namespace PainKiller.PowerCommands.MyExampleCommands.Commands.Templates;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Description of your command...",
                         example: "demo")]
public class NameCommand : CommandBase<PowerCommandsConfiguration>
{
    public NameCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        //insert your code here
        return Ok();
    }
}