namespace PainKiller.PowerCommands.Configuration.DomainObjects
{
    public class PowerCommandsConfiguration
    {
        public Metadata Metadata { get; set; } = new Metadata();
        public string[] Commands { get; set; } = new[]{""};

    }
}