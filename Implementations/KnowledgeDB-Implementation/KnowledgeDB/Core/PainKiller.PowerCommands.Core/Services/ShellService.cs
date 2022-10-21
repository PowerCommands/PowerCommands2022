using System.Diagnostics;
using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Services;

public class ShellService : IShellService
{
    private const int ImmediateReturn = 1000;
    private const int InfiniteWait = -1;
    private readonly ILogger _logger;

    private ShellService(ILogger logger) => _logger = logger;
    private static readonly Lazy<IShellService> Lazy = new(() => new ShellService(IPowerCommandServices.DefaultInstance!.Logger));
    public static IShellService Service => Lazy.Value;
    public void OpenDirectory(string directory)
    {
        _logger.LogInformation($"{nameof(ShellService)} {nameof(OpenDirectory)} {directory}");
        if (!Directory.Exists(directory)) return;
        Process.Start(new ProcessStartInfo { FileName = directory, UseShellExecute = true, Verb = "open" });
    }

    public void Execute(string programName, string arguments, string workingDirectory, Action<string,bool> writeFunction, string fileExtension = "exe", bool waitForExit = false, bool useShellExecute = false)
    {
        _logger.LogInformation($"{nameof(ShellService)} runs Execute with paramaters {programName} {arguments} {workingDirectory} {fileExtension} {waitForExit}");
        var extension = string.IsNullOrEmpty(fileExtension) ? "" : $".{fileExtension}";
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = useShellExecute,
            RedirectStandardOutput = !useShellExecute,
            FileName = $"{programName}{extension}",
            Arguments = arguments,
            WorkingDirectory = workingDirectory
        };

        var processAdd = Process.Start(startInfo);
        if (waitForExit)
        {
            var outputAdd = processAdd!.StandardOutput.ReadToEnd();
            writeFunction(outputAdd, true);
            _logger.LogInformation($"{nameof(ShellService)} output: {outputAdd}");
        }
        processAdd!.WaitForExit(waitForExit ? InfiniteWait : ImmediateReturn);
    }

    public void Execute(string programName, string arguments, string workingDirectory, string fileExtension = "exe", bool waitForExit = false, bool useShellExecute = false)
    {
        _logger.LogInformation($"{nameof(ShellService)} runs Execute with paramaters {programName} {arguments} {workingDirectory} {fileExtension} {waitForExit}");
        var extension = string.IsNullOrEmpty(fileExtension) ? "" : $".{fileExtension}";
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = useShellExecute,
            RedirectStandardOutput = true,
            FileName = $"{programName}{extension}",
            Arguments = arguments,
            WorkingDirectory = workingDirectory
        };
        var processAdd = Process.Start(startInfo);
        if (waitForExit)
        {
            var outputAdd = processAdd!.StandardOutput.ReadToEnd();
            Console.WriteLine(outputAdd);
            _logger.LogInformation($"{nameof(ShellService)} output: {outputAdd}");
        }
        processAdd!.WaitForExit(waitForExit ? InfiniteWait : ImmediateReturn);
    }
}