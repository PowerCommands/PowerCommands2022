using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("core|shell|folder|open")]
[PowerCommand(description: "Open a given directory or current working folder if argument is omitted",
    arguments: "Directory name: Argument is optional and could be omitted, if provided it must be a valid path do a directory",
    example: "directory|directory C:\\repos")]
public class DirCommand : CommandBase<CommandsConfiguration>
{
    public DirCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var directory = string.IsNullOrEmpty(input.Path) ? AppContext.BaseDirectory : input.Path;
        if (!Directory.Exists(directory)) return CreateBadParameterRunResult(input, $"Could not find directory \"{directory}\"");
        ShellService.Service.OpenDirectory(directory);
        WriteLine($"Open directory {directory}");
        return CreateRunResult(input);
    }
}