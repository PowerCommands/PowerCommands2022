using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(description: "View and manage the log", arguments: "action: show, delete, list", qutes:"filename: name of the file to be shown or deleted")]
[Tags("core|diagnostic|log|debug")]
public class LogCommand : CommandBase<CommandsConfiguration>
{
    public LogCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "list") List();
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private void List()
    {
        var dir = new DirectoryInfo(Configuration.Log.FilePath);
        foreach (var file in dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime)) WriteLine($"{file.Name} {file.LastWriteTime}");
        
    }
}