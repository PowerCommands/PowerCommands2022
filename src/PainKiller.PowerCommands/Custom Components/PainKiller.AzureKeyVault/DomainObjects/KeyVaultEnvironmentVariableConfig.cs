using PainKiller.AzureKeyVault.Contracts;

namespace PainKiller.AzureKeyVault.DomainObjects
{
    public class KeyVaultEnvironmentVariableConfig : IKeyVaultConfig
    {
        public KeyVaultEnvironmentVariableConfig()
        {
            KeyVaultName = Environment.GetEnvironmentVariable(EnvironmentVariableNames.KeyVaultName, EnvironmentVariableTarget.User) ?? "";
            ClientId = Environment.GetEnvironmentVariable(EnvironmentVariableNames.ClientId, EnvironmentVariableTarget.User) ?? "";
            ClientSecret = Environment.GetEnvironmentVariable(EnvironmentVariableNames.ClientSecret, EnvironmentVariableTarget.User) ?? "";
            TenantId = Environment.GetEnvironmentVariable(EnvironmentVariableNames.TenantId, EnvironmentVariableTarget.User) ?? "";
            KvUri = "https://" + KeyVaultName + ".vault.azure.net";
        }

        public string KeyVaultName { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string TenantId { get; }
        public string KvUri { get; }
    }
}