using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[PowerCommand(description: "Open a given directory or current working folder if argument is omitted",
    arguments: "Directory name:<directory name>",
    example: "dir|dir C:\\repos")]
public class DirCommand : CommandBase<CommandsConfiguration>
{
    public DirCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var directory = string.IsNullOrEmpty(Input.Path) ? AppContext.BaseDirectory : Input.Path;
        if (!Directory.Exists(directory)) return CreateBadParameterRunResult($"Could not find directory \"{directory}\"");
        ShellService.Service.OpenDirectory(directory);
        WriteLine($"Open directory {directory}");
        return CreateRunResult();
    }
}