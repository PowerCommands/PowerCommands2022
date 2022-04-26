using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;
namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("example|shell|git|execute|program|util")]
[PowerCommand(description: "Example shows how to execute a external program, in this case git, commmit and push your repository, path to repository is in the configuratin file",
    arguments: "operation type:commit|push",
    argumentMandatory: true,
    qutes: "comment: on commit a comment should be provided, not used when pushing, comment defaults to \"refactoring\" if omitted.",
    example: "git commit \"Bugfix\"|git push")]

public class GitCommand : CommandBase<PowerCommandsConfiguration>
{
    public GitCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "commit") Commit(input.SingleQuote);
        if (input.SingleArgument == "push") Push();
        return CreateRunResult(input);
    }
    public void Commit(string comment)
    {
        if (string.IsNullOrEmpty(comment)) comment = "\"refactoring\"";
        ShellService.Service.Execute("git", "add .", Configuration.DefaultGitRepositoryPath, WriteLine);
        ShellService.Service.Execute("git", $"commit -m \"{comment}\"", Configuration.DefaultGitRepositoryPath, WriteLine);
    }
    public void Push() => ShellService.Service.Execute("git", "push", Configuration.DefaultGitRepositoryPath, WriteLine);
}