using PainKiller.PowerCommands.MyExampleCommands.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("Demo|Example")]
[PowerCommand(  description: "Demonstration command just to show your input of arguments, quotes and flags",
                arguments: "<value>",
                qutes:"<value>",
                flags:"<value>",
                example: "input myArgument \"myQuote\" --myFlag")]
public class InputCommand : CommandBase<PowerCommandsConfiguration>
{
    public InputCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        if (input.Arguments.Length > 0) WriteLine($"Arguments: {string.Join(' ', input.Arguments)}");
        if (input.Quotes.Length > 0) WriteLine($"Quotes: {string.Join(' ', input.Quotes)}");
        if (input.Arguments.Length > 0) WriteLine($"Flags: {string.Join(' ', input.Flags)}");
        
        return CreateRunResult(input);
    }
}