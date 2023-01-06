namespace $safeprojectname$.DomainObjects;

public class EncryptionConfiguration
{
    public string SharedSecretEnvironmentKey { get; set; } = "";
    public string SharedSecretSalt { get; set; } = "";
}