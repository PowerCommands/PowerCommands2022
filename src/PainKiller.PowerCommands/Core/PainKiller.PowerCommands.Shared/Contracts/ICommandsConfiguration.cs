using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface ICommandsConfiguration
{
    bool ShowDiagnosticInformation { get; set; }
    Metadata Metadata { get; set; }
    LogComponentConfiguration Log { get; set; }
    List<BaseComponentConfiguration> Components { get; set; }
    public SecretConfiguration Secret { get; set; }
}