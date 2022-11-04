using PainKiller.PowerCommands.MyExampleCommands.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(  description: "Demonstration command just to show your input of arguments, quotes and flags",
                arguments: "<value>",
                qutes:"<value>",
                flags:"<value>",
                example: "input myArgument \"myQuote\" --myFlag")]
public class InputCommand : CommandBase<PowerCommandsConfiguration>
{
    public InputCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        if (Input.Arguments.Length > 0) WriteLine($"Arguments: {string.Join(' ', Input.Arguments)}");
        if (Input.Quotes.Length > 0) WriteLine($"Quotes: {string.Join(' ', Input.Quotes)}");
        if (Input.Arguments.Length > 0) WriteLine($"Flags: {string.Join(' ', Input.Flags)}");
        
        return Ok();
    }
}