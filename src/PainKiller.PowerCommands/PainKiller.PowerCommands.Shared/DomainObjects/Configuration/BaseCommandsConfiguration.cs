namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration
{
    public class BaseCommandsConfiguration
    {
        public bool ShowDiagnosticInformation { get; set; }
        public Metadata Metadata { get; set; } = new Metadata();
        public string[] Commands { get; set; } = new[]{""};
        public LogComponentConfiguration Log { get; set; } = new LogComponentConfiguration();
    }
}