namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration
{
    public class SecretItemConfiguration
    {
        public SecretItemConfiguration() => Options.Add("target", "User");
        public SecretItemConfiguration(EnvironmentVariableTarget target) => Options.Add("target", $"{target}");
        public string Name { get; set; } = "command-name-password";
        public Dictionary<string, string> Options { get; set; } = new();
    }
}