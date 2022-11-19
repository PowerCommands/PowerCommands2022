namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandTest(tests: "myArgument \"myQuote\" --myOption")]
[PowerCommandDesign(  description: "Demonstration command just to show your input of arguments, quotes and options",
                arguments: "<value>",
                quotes:"<value>",
                options:"<value>",
                example: "input myArgument \"myQuote\" --myOption")]
public class InputCommand : CommandBase<PowerCommandsConfiguration>
{
    public InputCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        if (Input.Arguments.Length > 0) WriteLine($"Arguments: {string.Join(' ', Input.Arguments)}");
        if (Input.Quotes.Length > 0) WriteLine($"Quotes: {string.Join(' ', Input.Quotes)}");
        if (Input.Arguments.Length > 0) WriteLine($"Options: {string.Join(' ', Input.Options)}");
        
        return Ok();
    }
}