namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Try out how flag works, first flag requires a value, second does not.",
                          flags: "!mandatory|optional",
                        example: "//This will work|flag --mandatory TheVal|//flag value could be an argument or a quote, but the quote should not include white space|flag --mandatory \"TheValue\"|//This is also ok|flag --mandatory value --optional|//This will not work|flag --mandatory|//This will work, --mandatory flag could be omitted|flag --optional|//This will work, --mandatory flag could be omitted and the --optional flag could have a value (but must not have it)|flag --optional theValue")]
public class FlagCommand : CommandBase<CommandsConfiguration>
{
    public FlagCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine($"mandatory: {Input.GetFlagValue("mandatory")}");
        WriteLine($"optional: {Input.GetFlagValue("optional")}");
        WriteLine($"Single Quote: {Input.SingleQuote}");
        return Ok();
    }
}