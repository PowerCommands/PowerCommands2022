using PainKiller.PowerCommands.Core.Extensions;
namespace PainKiller.PowerCommands.Core.Commands;

[Tags("core|diagnostic|log|debug|zip|compression|temp")]
[PowerCommand(       description: "View and manage the log",
                       arguments: "view|archive|list (default)",
                           qutes: "process name:<name>",
                      suggestion: "view",
                         example: "log list|log archive|log view|process created")]
public class LogCommand : CommandBase<CommandsConfiguration>
{
    public LogCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleArgument) || input.SingleArgument == "list") List();
        if (input.SingleArgument == "archive") Archive();
        if (input.SingleArgument == "view") View();
        if (input.SingleArgument == "process") ProcessLog($"{input.Quotes}");
        
        return CreateRunResult(input);
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
    private void Archive() => WriteLine(Configuration.Log.ArchiveLogFiles());
    private void View()
    {
        foreach (var line in Configuration.Log.ToLines()) Console.WriteLine(line);
    }

    private void ProcessLog(string processTag)
    {
        foreach (var line in Configuration.Log.GetProcessLog(processTag)) Console.WriteLine(line);
    }
}