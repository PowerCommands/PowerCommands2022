using PainKiller.AzureKeyVault.Contracts;

namespace PainKiller.AzureKeyVault.DomainObjects
{
    public class KeyVaultEnvironmentVariableConfig : IKeyVaultConfig
    {
        public KeyVaultEnvironmentVariableConfig()
        {
            KeyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME", EnvironmentVariableTarget.User) ?? "";
            ClientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID", EnvironmentVariableTarget.User) ?? "";
            ClientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET", EnvironmentVariableTarget.User) ?? "";
            TenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID", EnvironmentVariableTarget.User) ?? "";
            KvUri = "https://" + KeyVaultName + ".vault.azure.net";
        }

        public string KeyVaultName { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string TenantId { get; }
        public string KvUri { get; }
    }
}