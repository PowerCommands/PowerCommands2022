namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

[Tags("example|shell|git|execute|program|util")]
[PowerCommand(description: "Example shows how to execute a external program, in this case git, commmit and push your repository, path to repository is in the configuratin file",
    arguments: "commit|push",
    argumentMandatory: true,
    qutes: "\"<comment>\" defaults to \"refactoring\" if omitted, only used with commit.",
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