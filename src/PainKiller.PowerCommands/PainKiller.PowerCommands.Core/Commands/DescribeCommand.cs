using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(       description: "With help command you will be shown the provided description of the command, argument and quotes input parameters",
                       arguments: "name: Uses one argument, wich is the name of the command you want do display help for",
                defaultParameter: "exit",
                         example: "describe exit|describe cls|describe log")]
[Tags("core|help")]
public class DescribeCommand : CommandBase<CommandsConfiguration>
{
    public DescribeCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var identifier = string.IsNullOrEmpty(input.SingleArgument) ? "describe" : input.SingleArgument;
        var command = IPowerCommandsRuntime.DefaultInstance?.Commands.FirstOrDefault(c => c.Identifier == identifier);
        if (command == null)
        {
            if(input.Identifier != nameof(DescribeCommand).ToLower().Replace("command","")) WriteLine($"Command with identifier:{input.Identifier} not found");
            WriteLine("Found commands are:", addToOutput: false);
            foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance?.Commands!) WriteLine(consoleCommand.Identifier, addToOutput: false);
            return CreateRunResult(this, input, RunResultStatus.Ok);
        }
        HelpService.Service.ShowHelp(command);
        Console.WriteLine();
        WriteHeadLine("To se all availible commands type commands");
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}