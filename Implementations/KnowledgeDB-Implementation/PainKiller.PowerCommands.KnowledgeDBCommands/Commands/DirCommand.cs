using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

[PowerCommandDesign(description: "Open a given directory or current working folder if argument is omitted",
    arguments: "<directory name>",
    options: "app",
    example: "dir|dir C:\\repos")]
public class DirCommand : CommandBase<CommandsConfiguration>
{
    public DirCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var directory = string.IsNullOrEmpty(Input.Path) ? (Input.HasOption("app") ? ConfigurationGlobals.ApplicationDataFolder : AppContext.BaseDirectory) : Input.Path;
        if (!Directory.Exists(directory)) return BadParameterError($"Could not find directory \"{directory}\"");
        ShellService.Service.OpenDirectory(directory);
        WriteLine($"Open directory {directory}");
        return Ok();
    }
}