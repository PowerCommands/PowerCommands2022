using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Try out how flag and argument or quote parameter works together",
    arguments: "<value>",
    quotes: "<value>",
    flags: "<value>",
    example: "flag --myFlag myArgument|flag --myFlag \"myQuote\"")]
public class FlagCommand : CommandBase<CommandsConfiguration>
{
    public FlagCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var flag = Input.Flags.FirstOrDefault();
        if (flag == null) return BadParameterError("Could not fine any flag in your input, a flag is an argument starting with --");
        WriteHeadLine("Flag demo, using only one flag");
        WriteLine($"Flag: {Input.Flags.First()}");
        WriteLine($"Flag value: {Input.GetFlagValue(flag)}");
        return Ok();
    }
}