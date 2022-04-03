using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration
{
    public class BasicCommandsConfiguration : IBasicCommandsConfiguration
    {
        public bool ShowDiagnosticInformation { get; set; } = true;
        public Metadata Metadata { get; set; } = new();
        public LogComponentConfiguration Log { get; set; } = new();
        public List<BaseComponentConfiguration> Components { get; set; } = new() {new BaseComponentConfiguration {Name = "PainKiller Core", Component = "PainKiller Core", Checksum = "e6d2d6cb64863e9dc68a9602f83bcfde"}};
    }
}