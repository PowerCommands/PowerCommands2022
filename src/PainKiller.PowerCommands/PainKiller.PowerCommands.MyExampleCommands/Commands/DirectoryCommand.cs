using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Input a valid path to a directory and the commmand will analyse the directory and its subdirectories, this command showing the use of Path property on the input and how you could display a progress with overwriting one line in the iteration",
                arguments: "path: a valid dirctory path, spaces in the path are allowed en will be merged to the Path property on the Input instance",
                    qutes: "size: Let the iteratin sort out big files and display them, size is in megabytes and defaults to 10 (MB) if omitted",
                  example: "dirctory C:\\Program Files",
                 useAsync: true)]
[Tags("example|iteration|async|line|display|inline")]
public class DirectoryCommand : CommandBase<CommandsConfiguration>
{
    private List<string> _bigFiles = new();
    private long _minFileSize = 10;
    const int OneMegabyte = 1048576;
    public DirectoryCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override async Task<RunResult> RunAsync(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleArgument) || !Directory.Exists(input.Path)) return CreateBadParameterRunResult(this, input, $"{input.SingleQuote} must be a valid directory path");
        if (int.TryParse(input.SingleQuote, out var number)) _minFileSize = number;
        var rootDirectory = new DirectoryInfo(input.Path);

        await RunIterations(rootDirectory);
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
    private async Task RunIterations(DirectoryInfo rootDirectory)
    {
        await Task.Yield();
        TraverseDirectory(rootDirectory);
        OverwritePreviousLine("Big files found:");
        foreach (var bigFile in _bigFiles) WriteLine(bigFile);
        Console.Write("\nDone!\npcm>");
    }
    private void TraverseDirectory(DirectoryInfo startDirectory)
    {
        foreach (var subDirectory in startDirectory.GetDirectories())
        {
            OverwritePreviousLine(subDirectory.Name);
            TraverseDirectory(subDirectory);
        }

        foreach (var fileInfo in startDirectory.GetFiles())
        {
            OverwritePreviousLine(fileInfo.Name);
            var fileSizeInMegaBytes = fileInfo.Length > OneMegabyte ? fileInfo.Length/OneMegabyte : 0;
            if (fileSizeInMegaBytes > _minFileSize) _bigFiles.Add($"{startDirectory.FullName}\\{fileInfo.Name} {fileSizeInMegaBytes} Megabytes");
        }
    }
}