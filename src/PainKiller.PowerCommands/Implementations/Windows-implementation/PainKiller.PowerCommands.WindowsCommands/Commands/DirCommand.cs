using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.WindowsCommands.Commands;

[Tags("core|shell|folder|open|util")]
[PowerCommand(description: "Open a given directory or current working folder if argument is omitted",
    arguments: "<directory name>",
    qutes: "<directory name>",
    example: "dir|dir C:\\repos|dir \"C:\\repos\"")]
public class DirCommand : CommandBase<CommandsConfiguration>
{
    public DirCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var directory = string.IsNullOrEmpty(input.Path) ? AppContext.BaseDirectory : input.Path;
        if (!string.IsNullOrEmpty(input.SingleArgument)) directory = input.SingleArgument;
        if (!string.IsNullOrEmpty(input.SingleQuote)) directory = input.SingleQuote;
        if (!Directory.Exists(directory)) return CreateBadParameterRunResult(input, $"Could not find directory \"{directory}\"");
        ShellService.Service.OpenDirectory(directory);
        WriteLine($"Open directory {directory}");
        return CreateRunResult(input);
    }
}