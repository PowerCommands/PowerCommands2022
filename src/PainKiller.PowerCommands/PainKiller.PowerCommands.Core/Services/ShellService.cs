﻿using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Services;

public class ShellService : IShellService
{
    private ShellService() { }
    private static readonly Lazy<IShellService> Lazy = new(() => new ShellService());
    public static IShellService Service => Lazy.Value;
    public void Execute(string programName, string arguments, string workingDirectory, Action<string,bool> writeFunction, string fileExtension = "exe")
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = $"{programName}.{fileExtension}",
            Arguments = arguments,
            WorkingDirectory = workingDirectory
        };

        var processAdd = System.Diagnostics.Process.Start(startInfo);
        var outputAdd = processAdd!.StandardOutput.ReadToEnd();
        writeFunction(outputAdd, true);
        processAdd.WaitForExit();
    }

    public void Execute(string programName, string arguments, string workingDirectory, string fileExtension = "exe")
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = $"{programName}.{fileExtension}",
            Arguments = arguments,
            WorkingDirectory = workingDirectory
        };

        var processAdd = System.Diagnostics.Process.Start(startInfo);
        var outputAdd = processAdd!.StandardOutput.ReadToEnd();
        Console.WriteLine(outputAdd);
        processAdd.WaitForExit();
    }
}