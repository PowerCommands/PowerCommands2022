using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.WindowsCommands.Configuration;

namespace PainKiller.PowerCommands.WindowsCommands.Commands;

[Tags("environment variables|path")]
[PowerCommand(description: "Display registred paths in Enviroment variable Path",
    arguments: "<create>",
    example: "path|path \"<path to directory>\"")]
public class PathCommand : CommandBase<PowerCommandsConfiguration>
{
    public PathCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (Input.SingleArgument == "create")
        {
            CreatePath($"{Input.SingleQuote}");
            return CreateRunResult();
        }
        ShowPaths();
        return CreateRunResult();
    }

    private void ShowPaths()
    {
        WriteHeadLine("Registered paths");
        var paths = EnvironmentService.Service.GetEnvironmentVariable("Path", target: EnvironmentVariableTarget.Machine).Split(';');
        foreach (var path in paths)
            WriteLine($"{path}/t[MACHINE]");
        var userPaths = EnvironmentService.Service.GetEnvironmentVariable("Path", target: EnvironmentVariableTarget.Machine).Split(';');
        foreach (var path in userPaths)
            WriteLine($"{path}/t[USER]");
    }

    private void CreatePath(string path)
    {
        var paths = EnvironmentService.Service.GetEnvironmentVariable("Path", target: EnvironmentVariableTarget.User).Split(';').ToList();
        paths.Add(path);
        var envPath = string.Join(';', paths);
        EnvironmentService.Service.SetEnvironmentVariable("Path", envPath, target:EnvironmentVariableTarget.User);
        WriteHeadLine($"{path} added to PATH variable on local machine");
        WriteLine(envPath);
    }
}