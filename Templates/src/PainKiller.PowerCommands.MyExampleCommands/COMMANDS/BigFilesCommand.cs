namespace $safeprojectname$.Commands;

[PowerCommandDesign(description: "Input a valid path to a directory and the commmand will analyse the directory and its subdirectories\nThis command showing the use of Path property on the input and how you could display a progress with overwriting one line in the iteration.\nSpaces in the path are allowed en will be merged to the Path property on the Input instance",
                    quotes: "Path",
                    options: "megabytes",
                suggestions: "path",
                  example: "bigfiles --path \"C:\\Repos|bigfiles\"|bigfiles \"C:\\Repos\" --megabytes 1024",
                 useAsync: true)]
public class BigFilesCommand : CdCommand
{
    private readonly List<string> _bigFiles = new();
    private long _minFileSize = 3;
    const int OneMegabyte = 1048576;
    public BigFilesCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override async Task<RunResult> RunAsync()
    {
        var megaBytes = Input.GetOptionValue("megabytes");
        var path = Input.Path;
        if (string.IsNullOrEmpty(path)) path = WorkingDirectory;

        if (!Directory.Exists(path)) return BadParameterError($"{path} must be a valid directory path");
        if (int.TryParse(megaBytes, out var number)) _minFileSize = number;
        var rootDirectory = new DirectoryInfo(path);
        await RunIterations(rootDirectory);
        return Ok();
    }
    private async Task RunIterations(DirectoryInfo rootDirectory)
    {
        await Task.Yield();
        TraverseDirectory(rootDirectory);
        OverwritePreviousLine($"Big files found over {_minFileSize} MB:");
        foreach (var bigFile in _bigFiles) WriteLine(bigFile);
        Console.Write($"\nDone!\n{ConfigurationGlobals.Prompt}");
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
            var fileSizeInMegaBytes = fileInfo.Length > OneMegabyte ? fileInfo.Length / OneMegabyte : 0;
            if (fileSizeInMegaBytes > _minFileSize) _bigFiles.Add($"{startDirectory.FullName}\\{fileInfo.Name} {fileSizeInMegaBytes} Megabytes");
        }
    }
}