using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("help|configuration|util")]
[PowerCommand( description: "Config command is a util to help you build a default yaml configuration file, practical when you adding new configuration elements to the PowerCommandsConfiguration class",
                 arguments: "create|edit",
                suggestion: "edit",
                   example: "config|config create|config edit")]
public class ConfigCommand : CommandBase<PowerCommandsConfiguration>
{
    public ConfigCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
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
        if (input.SingleArgument == "edit")
        {
            try { ShellService.Service.Execute(Configuration.CodeEditor, arguments: $"{Path.Combine(AppContext.BaseDirectory, $"{nameof(PowerCommandsConfiguration)}.yaml")}", workingDirectory: "", WriteLine, fileExtension: ""); }
            catch (Exception) { return CreateBadParameterRunResult(input, "Your editor must be included in Path environment variables"); }
        }
        var configurationRows = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, $"{nameof(PowerCommandsConfiguration)}.yaml")).Split('\n');
        foreach (var configurationRow in configurationRows) Console.WriteLine(configurationRow);
        return CreateRunResult(input);
    }
}