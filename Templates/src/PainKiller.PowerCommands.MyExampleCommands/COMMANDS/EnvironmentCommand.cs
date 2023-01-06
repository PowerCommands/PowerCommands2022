namespace $safeprojectname$.Commands;

[PowerCommandDesign(description: "View environment variable or create environment variable",
                          options: "!get",
                        example: "environment --get OS")]
public class EnvironmentCommand : CommandBase<CommandsConfiguration>
{
    public EnvironmentCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if(Input.HasOption("get")) WriteLine(Configuration.Environment.GetValue(Input.GetOptionValue("get")));
        return Ok();
    }
}