using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class GitCommand : CommandBase<PowerCommandsConfiguration>
{
    public GitCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if(input.SingleArgument == "commit") Commit(input.SingleArgument);
        if(input.SingleArgument == "push") Push();
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    public void Commit(string comment)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = "git.exe",
            Arguments = $"add .",
            WorkingDirectory = Configuration.DefaultGitRepositoryPath
        };

        var processAdd = System.Diagnostics.Process.Start(startInfo);
        var outputAdd = processAdd!.StandardOutput.ReadToEnd();
        WriteLine(outputAdd);
        processAdd.WaitForExit();


        startInfo.Arguments = $"commit -m {comment}";

        var processCommit = System.Diagnostics.Process.Start(startInfo);
        var outputCommit = processCommit!.StandardOutput.ReadToEnd();
        WriteLine(outputCommit);
        processCommit.WaitForExit();
    }

    public void Push()
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = "git.exe",
            Arguments = "push",
            WorkingDirectory = Configuration.DefaultGitRepositoryPath
        };

        var processAdd = System.Diagnostics.Process.Start(startInfo);
        var outputAdd = processAdd!.StandardOutput.ReadToEnd();
        WriteLine(outputAdd);
        processAdd.WaitForExit();
    }
}