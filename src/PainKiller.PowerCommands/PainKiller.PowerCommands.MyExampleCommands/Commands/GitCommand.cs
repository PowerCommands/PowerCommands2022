using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("example|shell|git|execute|program|util")]
[PowerCommand(description: "Example shows how to execute a external program, in this case git, commit and push your repository, path to repository is in the configuration file",
                arguments: "commit|push|status",
        argumentMandatory: true,
                    qutes: "\"<comment>\" defaults to \"refactoring\" if omitted, only used with commit.",
                  example: "git commit \"Bugfix\"|git push|git status")]

public class GitCommand : CommandBase<PowerCommandsConfiguration>
{
    public GitCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        switch (input.SingleArgument)
        {
            case "commit":
                Commit(input.SingleQuote);
                break;
            case "push":
                Push();
                break;
            case "status":
                Status();
                break;
            default: 
                return CreateBadParameterRunResult(input, $"Parameter {input.SingleArgument} not supported");
        }
        return CreateRunResult(input);
    }
    public void Commit(string comment)
    {
        if (string.IsNullOrEmpty(comment)) comment = "\"refactoring\"";
        ShellService.Service.Execute("git", "add .", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
        ShellService.Service.Execute("git", $"commit -m \"{comment}\"", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
    }
    public void Push() => ShellService.Service.Execute("git", "push", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);

    public void Status() => ShellService.Service.Execute("git", "status", Configuration.DefaultGitRepositoryPath, WriteLine, waitForExit: true);
}