using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Core.Commands;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

[PowerCommandDesign(description: "List the content of the working directory or this applications app directory, with the option to open the directory with the File explorer ",
                        options: "open|app",
                        example: "//List the content and open the current working directory|dir --open|//Open the AppData roaming directory|dir --app --open")]
public class DirCommand : CdCommand
{
    public DirCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var directory = Input.HasOption("app") ? ConfigurationGlobals.ApplicationDataFolder : CdCommand.WorkingDirectory;
        if (!Directory.Exists(directory)) return BadParameterError($"Could not find directory \"{directory}\"");
        if(HasOption("open")) ShellService.Service.OpenDirectory(directory);
        ShowDirectories(directory);
        return Ok();
    }
}