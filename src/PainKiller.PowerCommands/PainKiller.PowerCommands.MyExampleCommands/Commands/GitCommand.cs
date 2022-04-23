using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Example shows how to execute a external program, in this case git, commmit and push your repository, path to repository is in the configuratin file",
                arguments: "operation type: commit or push",
                    qutes: "comment: on commit a comment should be provided, not used when pushing, comment defaults to \"refatoring\" if omitted.",
                  example: "git commit \"Bugfix\"|git push")]
[Tags("example|shell|git|execute|program")]
public class GitCommand : CommandBase<PowerCommandsConfiguration>
{
    public GitCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if(input.SingleArgument == "commit") Commit(input.SingleQuote);
        if(input.SingleArgument == "push") Push();
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }

    public void Commit(string comment)
    {
        if (string.IsNullOrEmpty(comment)) comment = "\"refactoring\"";
        ShellService.Service.Execute("git", "add .", Configuration.DefaultGitRepositoryPath, WriteLine);
        ShellService.Service.Execute("git", $"commit -m \"{comment}\"", Configuration.DefaultGitRepositoryPath, WriteLine);
    }

    public void Push()
    {
        ShellService.Service.Execute("git", "push", Configuration.DefaultGitRepositoryPath, WriteLine);
    }
}