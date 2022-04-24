using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(       description: "View and manage the log",
                       arguments: "action: view, archive, list (default if omitted)",
                           qutes: "filename: name of the file to be viewed",
                defaultParameter: "view",
                         example: "log list|log archive|log view")]
[Tags("core|diagnostic|log|debug|zip|compression|temp")]
public class LogCommand : CommandBase<CommandsConfiguration>
{
    public LogCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleArgument) || input.SingleArgument == "list") List();
        if (input.SingleArgument == "archive") Archive();
        if (input.SingleArgument == "view") View();
        
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private void List()
    {
        var dir = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, Configuration.Log.FilePath));
        foreach (var file in dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime)) WriteLine($"{file.Name} {file.LastWriteTime}");
        Console.WriteLine();
        WriteHeadLine("To view current logfile type log view");
        WriteHeadLine("Example");
        Console.WriteLine("log view");

        Console.WriteLine();
        WriteHeadLine("To archive the logs into a zipfile type log archive");
        WriteHeadLine("Example");
        Console.WriteLine("log archive");
    }

    private void Archive()
    {
        WriteLine(Configuration.Log.ArchiveLogFiles());
    }

    private void View()
    {
        var lines = Configuration.Log.ToLines();
        foreach (var line in lines) Console.WriteLine(line);
    }
}