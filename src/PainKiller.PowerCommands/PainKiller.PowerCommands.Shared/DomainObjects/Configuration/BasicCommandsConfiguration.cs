namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration
{
    public class BasicCommandsConfiguration
    {
        public bool ShowDiagnosticInformation { get; set; }
        public Metadata Metadata { get; set; } = new();
        public LogComponentConfiguration Log { get; set; } = new();
        public List<BaseComponentConfiguration> Components { get; set; } = new();
    }
}