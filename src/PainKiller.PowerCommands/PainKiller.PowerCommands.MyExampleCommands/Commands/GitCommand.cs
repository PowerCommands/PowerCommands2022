namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Example shows how to execute a external program, in this case git, commit and push your repository, path to repository is in the configuration file",
                arguments: "!commit|push|status",
                    quotes: "\"<comment>\" defaults to \"refactoring\" if omitted, only used with commit.",
                  example: "//Add and commit|git commit \"Bugfix\"|//Performs a push to Git repo|git push|//Git status of the configured git repo|git status|//Show local path to repo|git repo")]

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
                return BadParameterError($"Parameter {Input.SingleArgument} not supported");
        }
        return Ok();
    }
    public void Commit(string comment)
    {
        if (string.IsNullOrEmpty(comment)) comment = "\"refactoring\"";
        ShellService.Service.Execute("git", "add .", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"add . \"{comment}\"");
        ShellService.Service.Execute("git", $"commit -m \"{comment}\"", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"commit m \"{comment}\"");
    }
    public void Push()
    {
        ShellService.Service.Execute("git", "push", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", "push");
    }

    public void Status()
    { 
        WriteHeadLine($"Current Git repository: {Configuration.DefaultGitRepositoryPath}\n");
        ShellService.Service.Execute("git", "status", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        WriteProcessLog("GIT", $"status"); }

    public void Repo()
    {
        WriteLine($"Local repo path: {Configuration.DefaultGitRepositoryPath}");
        WriteProcessLog("GIT", $"Local repo path: {Configuration.DefaultGitRepositoryPath}");
    }
}