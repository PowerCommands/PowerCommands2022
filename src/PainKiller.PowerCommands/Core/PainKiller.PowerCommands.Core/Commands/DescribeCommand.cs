using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Contracts;
namespace PainKiller.PowerCommands.Core.Commands;

[Tags("core|help")]
[PowerCommand(       description: "With help command you will be shown the provided description of the command, argument and quotes input parameters",
                       arguments: "<command name>",
                      suggestion: "cls",
                         example: "describe exit|describe cls|describe log")]
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
            return CreateRunResult(input);
        }
        HelpService.Service.ShowHelp(command);
        Console.WriteLine();
        WriteHeadLine("To se all availible commands type commands");
        return CreateRunResult(input);
    }
}