namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(       description: "View and manage the log",
                       arguments: "view|archive|list (default)",
                           qutes: "process name:<name>",
                      suggestion: "view",
                         example: "//View a list with all the logfiles|log list|//Archive the logs into a zip file.|log archive|//View content of the current log|log view|//Filter the log show only posts matching the provided process tag, this requires that you are using process tags when logging in your command(s).|log process created")]
public class LogCommand : CommandBase<CommandsConfiguration>
{
    public LogCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (string.IsNullOrEmpty(Input.SingleArgument) || Input.SingleArgument == "list") List();
        if (Input.SingleArgument == "archive") Archive();
        if (Input.SingleArgument == "view") View();
        if (Input.SingleArgument == "process") ProcessLog($"{Input.Quotes}");
        
        return CreateRunResult();
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
        WriteHeadLine("To archive the logs into a zip file type log archive");
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