using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Example shows how to execute a external program, in this case git, commit and push your repository, path to repository is in the configuration file",
                arguments: "commit|push|status",
        argumentMandatory: true,
                    qutes: "\"<comment>\" defaults to \"refactoring\" if omitted, only used with commit.",
                  example: "/*Add and commit*/|git commit \"Bugfix\"|/*Performs a push to Git repo*/|git push|/*Git status of the configured git repo*/|git status|/*Show local path to repo*/|git repo")]

public class GitCommand : CommandBase<PowerCommandsConfiguration>
{
    public GitCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        switch (Input.SingleArgument)
        {
            case "commit":
                Commit(Input.SingleQuote);
                break;
            case "push":
                Push();
                break;
            case "status":
                Status();
                break;
            case "repo":
                Repo();
                break;
            default: 
                return CreateBadParameterRunResult($"Parameter {Input.SingleArgument} not supported");
        }
        return CreateRunResult();
    }
    public void Commit(string comment)
    {
        if (string.IsNullOrEmpty(comment)) comment = "\"refactoring\"";
        ShellService.Service.Execute("git", "add .", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        ShellService.Service.Execute("git", $"commit -m \"{comment}\"", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
    }
    public void Push() => ShellService.Service.Execute("git", "push", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);

    public void Status() => ShellService.Service.Execute("git", "status", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
    public void Repo() => WriteLine($"Local repo path: {Configuration.DefaultGitRepositoryPath}");
}