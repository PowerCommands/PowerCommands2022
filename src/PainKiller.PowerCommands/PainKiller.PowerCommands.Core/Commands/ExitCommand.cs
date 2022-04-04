using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

public class ExitCommand : CommandBase<CommandsConfiguration>
{
    public ExitCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input.Arguments.Length > 0 && input.Arguments.First().ToLower().StartsWith("y")) return new RunResult(this, input, output: "exit program", RunResultStatus.Quit);
        Console.WriteLine("Do you wanna quit the program? y/?");
        var response = $"{Console.ReadLine()}";
        if (response.ToLower().StartsWith("y")) return new RunResult(this, input, output: "exit program", RunResultStatus.Quit);
        return new RunResult(this, input, output: "No, dont exit the program", RunResultStatus.Ok);
    }
}