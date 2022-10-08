using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("Demo|Example|Flag|Parameter")]
[PowerCommand(description: "Try out how flag and argument or quote parameter works together",
    arguments: "<value>",
    qutes: "<value>",
    flags: "<value>",
    example: "flag --myFlag myArgument|flag --myFlag \"myQuote\"")]
public class FlagCommand : CommandBase<CommandsConfiguration>
{
    public FlagCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var flag = input.Flags.FirstOrDefault();
        if (flag == null) return CreateBadParameterRunResult(input, "Could not fine any flag in your input, a flag is an argument starting with --");
        WriteHeadLine("Flag demo, using only one flag");
        WriteLine($"Flag: {input.Flags.First()}");
        WriteLine($"Flag value: {input.GetFlagValue(flag)}");
        return CreateRunResult(input);
    }
}