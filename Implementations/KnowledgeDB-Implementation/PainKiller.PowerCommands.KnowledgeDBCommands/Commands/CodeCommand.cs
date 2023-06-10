using PainKiller.PowerCommands.Configuration.DomainObjects;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Description of your command...",
                         example: "demo")]
public class CodeCommand : CommandBase<PowerCommandsConfiguration>
{
    public CodeCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        try
        {
            var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"{nameof(KnowledgeDatabase)}.data");
            ShellService.Service.Execute(Configuration.CodeEditor, arguments: fileName, ConfigurationGlobals.ApplicationDataFolder, WriteLine, fileExtension: "");
        }
        catch (Exception) { return BadParameterError("Your editor must be included in Path environment variables"); }
        return Ok();
    }
}