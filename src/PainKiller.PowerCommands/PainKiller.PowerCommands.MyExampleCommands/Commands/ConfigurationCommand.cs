using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("help|configuration")]
[PowerCommand(       description: "Configuration command is a util to help you build a default yaml configuration file, practical when you adding new configuration elements to the PowerCommandsConfiguration class",
                       arguments: "create: To create a new yaml file in the root directory",
                defaultParameter: "create",
                         example: "configuration|configuration create")]
public class ConfigurationCommand : CommandBase<PowerCommandsConfiguration>
{
    public ConfigurationCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "create")
        {
            var componentManager = new ComponentManager<PowerCommandsConfiguration>(Configuration, PowerCommandServices.Service.Diagnostic);
            var configuration = new PowerCommandsConfiguration {Components = componentManager.AutofixConfigurationComponents(Configuration), Log = Configuration.Log, Metadata = Configuration.Metadata, ShowDiagnosticInformation = Configuration.ShowDiagnosticInformation,Secret = new() {Secrets = new List<SecretItemConfiguration>{new()}}};
            var fileName = ConfigurationService.Service.SaveChanges(configuration, inputFileName:"default.yaml");

            WriteLine($"A new default file named {fileName} has been created in the root directory");
            return CreateRunResult(input);
        }
        var configurationRows = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, $"{nameof(PowerCommandsConfiguration)}.yaml")).Split('\n');
        foreach (var configurationRow in configurationRows) Console.WriteLine(configurationRow);
        return CreateRunResult(input);
    }
}