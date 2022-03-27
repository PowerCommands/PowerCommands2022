using PainKiller.AzureKeyVault.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.AzureKeyVault.DomainObjects
{
    public class KeyVaultConfig : BaseComponentConfiguration, IKeyVaultConfig
    {
        public KeyVaultConfig()
        {
            Name = "AzureKeyVault";
            Component = $"{nameof(PainKiller.AzureKeyVault)}.dll";
            Checksum = "173831af7e77b8bd33e32fce0b4e646d";
        }
        public KeyVaultConfig(string keyVaultName, string clientId, string clientSecret, string tenantId, string kvUri) :this()
        {
            KeyVaultName = keyVaultName;
            ClientId = clientId;
            ClientSecret = clientSecret;
            TenantId = tenantId;
            KvUri = kvUri;
        }

        public KeyVaultConfig(string keyVaultName, string clientId, string clientSecret, string tenantId) : this()
        {
            KeyVaultName = keyVaultName;
            ClientId = clientId;
            ClientSecret = clientSecret;
            TenantId = tenantId;
            KvUri = "https://" + KeyVaultName + ".vault.azure.net";
        }

        public string KeyVaultName { get; } = "";
        public string ClientId { get; } = "";
        public string ClientSecret { get; } = "";
        public string TenantId { get; } = "";
        public string KvUri { get; } = "";
    }
}