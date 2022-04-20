using System.IO.Compression;
using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(description: "View and manage the log", arguments: "action: view, archive, list", qutes:"filename: name of the file to be viewed")]
[Tags("core|diagnostic|log|debug|zip|compression")]
public class LogCommand : CommandBase<CommandsConfiguration>
{
    public LogCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "list") List();
        if (input.SingleArgument == "archive") Archive(Configuration.Log.FilePath);
        if (input.SingleArgument == "view") View();
        
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    private void List()
    {
        var dir = new DirectoryInfo(Configuration.Log.FilePath);
        foreach (var file in dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime)) WriteLine($"{file.Name} {file.LastWriteTime}");
    }

    private void Archive(string directoryName)
    {
        var fileStamp = "archive".FormatFileTimestamp();
        var tempDirectory = $"{directoryName}\\{fileStamp}";
        var zipFileName = $"{Configuration.Log.FilePath}\\{fileStamp}.zip";

        var fileNames = Directory.GetFiles(Configuration.Log.FilePath, "*.log");

        Directory.CreateDirectory(tempDirectory);

        foreach (var fileName in fileNames)
        {
            var file = new FileInfo(fileName);
            File.Copy(fileName, $"{tempDirectory}\\{file.Name}");
            try { File.Delete(fileName); }
            catch (IOException e) { WriteLine($"The file {fileName} is the current logfile and could not be deleted."); }
            WriteLine($"{fileName} moved to archive directory {tempDirectory}");
        }
        ZipFile.CreateFromDirectory(tempDirectory, zipFileName);
        Directory.Delete(tempDirectory, recursive: true);
    }

    private void View()
    {
        var dir = new DirectoryInfo(Configuration.Log.FilePath);
        var currentFile = dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime).First();
        var tempFileName = $"{currentFile.Name}".FormatFileTimestamp();
        File.Copy(currentFile.FullName, tempFileName);
        
        var lines = File.ReadAllLines(tempFileName);
        foreach (var line in lines) Console.WriteLine(line);
        File.Delete(tempFileName);
    }
}