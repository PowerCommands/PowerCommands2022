using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(description: "With help command you will be shown the provided description of the command, argument and quotes input parameters", arguments: "name: Uses one argument, wich is the name of the command you want do display help for")]
public class HelpCommand : CommandBase<CommandsConfiguration>
{
    public HelpCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var command = IPowerCommandsRuntime.DefaultInstance.Commands.FirstOrDefault(c => c.Identifier == input.SingleArgument);
        if (command == null)
        {
            WriteLine($"Command with identifier:{input.Identifier} not found");
            WriteLine("Found commands are:", addToOutput: false);
            foreach (var consoleCommand in IPowerCommandsRuntime.DefaultInstance.Commands) WriteLine(consoleCommand.Identifier, addToOutput: false);
            return CreateRunResult(this, input, RunResultStatus.SyntaxError);
        }
        var descriptionAttribute = command.GetPowerCommandAttribute();
        WriteLine(descriptionAttribute.Description, addToOutput: false);
        WriteLine($"{nameof(descriptionAttribute.Arguments)}: {descriptionAttribute.Arguments}", addToOutput: false);
        WriteLine($"{nameof(descriptionAttribute.Qutes)}: {descriptionAttribute.Qutes}", addToOutput: false);

        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}