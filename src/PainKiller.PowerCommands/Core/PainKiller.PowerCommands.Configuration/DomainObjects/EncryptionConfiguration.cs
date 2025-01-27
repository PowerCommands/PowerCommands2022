namespace PainKiller.PowerCommands.Configuration.DomainObjects
{
    public class EncryptionConfiguration
    {
        public string SharedSecretEnvironmentKey { get; set; } = "";
        public string SharedSecretSalt { get; set; } = "";
        public int IterationCount { get; set; } = 10000;
        public int KeySize { get; set; } = 256;
    }
}