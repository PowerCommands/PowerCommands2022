using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(       description: "Configuration command is a util to help you build a default yaml configuration file, practical when you adding new configuration elements to the PowerCommandsConfiguration class",
                       arguments: "create: To create a new yaml file in the root directory",
                defaultParameter: "create",
                         example: "configuration|configuration")]
[Tags("help|configuration")]
public class ConfigurationCommand : CommandBase<PowerCommandsConfiguration>
{
    public ConfigurationCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "create")
        {
            var fileName = ConfigurationService.Service.SaveChanges(new PowerCommandsConfiguration());
            WriteLine($"A new default file named {fileName} has been created in the root directory");
            return CreateRunResult(this, input, RunResultStatus.Ok);
        }
        throw new IndexOutOfRangeException();
    }
}