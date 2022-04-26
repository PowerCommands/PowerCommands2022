using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("example|iteration|async|inline")]
[PowerCommand(description: "Input a valid path to a directory and the commmand will analyse the directory and its subdirectories\nThis command showing the use of Path property on the input and how you could display a progress with overwriting one line in the iteration.\nSpaces in the path are allowed en will be merged to the Path property on the Input instance",
                arguments: "path:<path to directory>",
        argumentMandatory: true,
                    qutes: "size in MB:<size (numeric)",
                  example: "directory C:\\Repos|directory C:\\Repos \"1024\"",
                 useAsync: true)]
public class BigFilesCommand : CommandBase<CommandsConfiguration>
{
    private readonly List<string> _bigFiles = new();
    private long _minFileSize = 10;
    const int OneMegabyte = 1048576;
    public BigFilesCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override async Task<RunResult> RunAsync(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleArgument) || !Directory.Exists(input.Path)) return CreateBadParameterRunResult(input, $"{input.SingleQuote} must be a valid directory path");
        if (int.TryParse(input.SingleQuote, out var number)) _minFileSize = number;
        var rootDirectory = new DirectoryInfo(input.Path);

        await RunIterations(rootDirectory);
        return CreateRunResult(input);
    }
    private async Task RunIterations(DirectoryInfo rootDirectory)
    {
        await Task.Yield();
        TraverseDirectory(rootDirectory);
        OverwritePreviousLine("Big files found:");
        foreach (var bigFile in _bigFiles) WriteLine(bigFile);
        Console.Write($"\nDone!\n{ConfigurationConstants.Prompt}");
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