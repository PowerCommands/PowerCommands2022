namespace $safeprojectname$.DomainObjects
{
    public class SecurityConfiguration
    {
        public EncryptionConfiguration Encryption { get; set; } = new();
    }
}