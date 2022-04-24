using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("help|configuration")]
[PowerCommand(       description: "Configuration command is a util to help you build a default yaml configuration file, practical when you adding new configuration elements to the PowerCommandsConfiguration class",
                       arguments: "create: To create a new yaml file in the root directory",
                defaultParameter: "create",
                         example: "configuration|configuration")]
public class ConfigurationCommand : CommandBase<PowerCommandsConfiguration>
{
    public ConfigurationCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleArgument)) return CreateBadParameterRunResult(input, "Missinga argument, example:\nconfiguration create");
        if (input.SingleArgument == "create")
        {
            var fileName = ConfigurationService.Service.SaveChanges(new PowerCommandsConfiguration());
            WriteLine($"A new default file named {fileName} has been created in the root directory");
        }
        return CreateRunResult(input);
    }
}