namespace $safeprojectname$.Commands;

[PowerCommandTest        (tests: "--mandatory value|!--optional babar --mandatory")]
[PowerCommandDesign(description: "Try out how options works, first option requires a value, second does not.",
                          options: "!mandatory|optional",
                        example: "//This will work|option --mandatory TheVal|//options value could be an argument or a quote, but the quote should not include white space|option --mandatory \"TheValue\"|//This is also ok|option --mandatory value --optional|//This will not work|option --mandatory|//This will work, --mandatory option could be omitted|option --optional|//This will work, --mandatory option could be omitted and the --optional option could have a value (but must not have it)|option --optional theValue")]
public class OptionCommand : CommandBase<CommandsConfiguration>
{
    public OptionCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine($"mandatory: {Input.GetOptionValue("mandatory")}");
        WriteLine($"optional: {Input.GetOptionValue("optional")}");
        WriteLine($"Single Quote: {Input.SingleQuote}");
        return Ok();
    }
}