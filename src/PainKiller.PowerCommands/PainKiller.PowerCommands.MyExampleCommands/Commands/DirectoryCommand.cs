using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("example|shell|folder|open")]
[PowerCommand(description: "Open a given directory or current working folder if argument is omitted",
    arguments: "Directory name: Argument is optional and could be omitted, if provided it must be a valid path do a directory",
    example: "shell music|shell games")]
public class DirectoryCommand : CommandBase<CommandsConfiguration>
{
    public DirectoryCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var directory = string.IsNullOrEmpty(input.Path) ? AppContext.BaseDirectory : input.Path;
        if (!Directory.Exists(directory)) return CreateBadParameterRunResult(input, $"Could not find directory \"{directory}\"");
        ShellService.Service.OpenDirectory(directory);
        return CreateRunResult(input);
    }
}