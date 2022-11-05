namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "View environment variable or create environment variable",
                          flags: "!get",
                        example: "environment --get OS")]
public class EnvironmentCommand : CommandBase<CommandsConfiguration>
{
    public EnvironmentCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if(Input.HasFlag("get")) WriteLine(Configuration.Environment.GetValue(Input.GetFlagValue("get")));
        return Ok();
    }
}