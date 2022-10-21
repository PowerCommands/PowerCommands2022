using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.KnowledgeDBCommands.Configuration;
using PainKiller.PowerCommands.KnowledgeDBCommands.DomainObjects;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

public class BackupCommand : CommandBase<PowerCommandsConfiguration>
{
    public BackupCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        var fileName = StorageService<KnowledgeDatabase>.Service.Backup();
        WriteLine($"File is backed up to {fileName}");
        return CreateRunResult();
    }
}