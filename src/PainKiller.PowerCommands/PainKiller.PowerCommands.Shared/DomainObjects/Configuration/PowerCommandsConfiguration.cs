namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration
{
    public class PowerCommandsConfiguration
    {
        public Metadata Metadata { get; set; } = new Metadata();
        public string[] Commands { get; set; } = new[]{""};

    }
}